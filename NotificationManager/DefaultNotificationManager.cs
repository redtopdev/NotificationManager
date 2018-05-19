namespace Engaze.NotificationManager
{
    using Engaze.NotificationManager.Contract;
    using Engaze.NotificationManager.PNM;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    public class DefaultNotificationManager : INotificationManager
    {
        IPushNotifier _pushNotifier;
        INotificationTaskQueue _taskQueue;
        private Predicate<string> _isValidGCMId;
        private Func<string, string, bool> _isRequester;
        private Func<ICollection<string>, string, bool> _isParticipantRemoved;

        public DefaultNotificationManager(IPushNotifier pushNotifier, INotificationTaskQueue taskQueue)
        {
            _pushNotifier = new GCMNotifier();
            _taskQueue = taskQueue;
            _isValidGCMId = Id => Id.ToUpper().CompareTo("TEMP") != 0;
            _isRequester = (userId, requesterId) => userId.ToString().ToLower().Equals(requesterId.ToString().ToLower());
            _isParticipantRemoved = (removedParticipantsGcms, GCMClientId) =>
            {
                var isRemoved = removedParticipantsGcms?.Contains(GCMClientId);
                return isRemoved.HasValue ? isRemoved.Value : false;
            };
        }

        public void NotifyEventCreation(EventSlim evt)
        {
            _taskQueue.Enqueue(new
            {
                RegistrationIds = evt.UserList.Where(user =>
                !_isRequester(user.UserId, evt.InitiatorId.ToString())
                && _isValidGCMId(user.GCMClientId)).Select(user => user.GCMClientId),
                NotificationData = PushMessageComposer.GetMessage(evt, "EventInvite")
            });
        }

        public void NotifyEventUpdate(EventSlim evt, List<string> removedParticipantsGcms)
        {
            _taskQueue.Enqueue(new
            {
                RegistrationIds = evt.UserList.Where(user =>
                !_isRequester(user.UserId, evt.InitiatorId.ToString()) &&
                !_isParticipantRemoved(removedParticipantsGcms, user.GCMClientId) &&
                _isValidGCMId(user.GCMClientId)).Select(user => user.GCMClientId),
                NotificationData = PushMessageComposer.GetMessage(evt, "EventUpdate")
            });
        }

        public void NotifyEventUpdateParticipants(EventSlim evt, List<string> removedParticipantsGcms)
        {
            _taskQueue.Enqueue(new
            {
                RegistrationIds = evt.UserList.Where(user =>
                !_isRequester(user.UserId, evt.InitiatorId.ToString()) &&
                !_isParticipantRemoved(removedParticipantsGcms, user.GCMClientId) &&
                _isValidGCMId(user.GCMClientId)).Select(user => user.GCMClientId),
                NotificationData = PushMessageComposer.GetMessage(evt, "EventUpdateParticipants")
            });
        }

        public void NotifyEventUpdateLocation(EventRequestSlim eventRequest, EventSlim evt)
        {
            _taskQueue.Enqueue(new
            {
                RegistrationIds = evt.UserList.Where(user =>
                !_isRequester(user.UserId, evt.InitiatorId.ToString()) &&
                _isValidGCMId(user.GCMClientId)).Select(user => user.GCMClientId),
                NotificationData = PushMessageComposer.GetMessageForUpdateLocation(evt, eventRequest, "EventUpdateLocation")
            });
        }

        public void NotifyAddParticipantToEvent(EventSlim evt, List<string> adddedParticipantGcms)
        {
            if (adddedParticipantGcms.Count != 0)
            {                //Notify newly added users of event invitation
                _taskQueue.Enqueue(new
                {
                    RegistrationIds = adddedParticipantGcms.Where(user => _isValidGCMId(user)),
                    NotificationData = PushMessageComposer.GetMessage(evt, "EventInvite")
                });
            }
        }

        public void NotifyRemoveParticipantFromEvent(EventSlim evt, List<string> removedParticipantGcms)
        {
            if (removedParticipantGcms.Count != 0)
            {
                _taskQueue.Enqueue(new
                {
                    RegistrationIds = removedParticipantGcms.Where(user => _isValidGCMId(user)),
                    NotificationData = PushMessageComposer.GetMessage(evt, "RemovedFromEvent")
                });
            }
        }

        public void NotifyEndEvent(EventSlim evt)
        {
            _taskQueue.Enqueue(new
            {
                RegistrationIds = evt.UserList.Where(user =>
                !_isRequester(user.UserId, evt.InitiatorId.ToString()) &&
                _isValidGCMId(user.GCMClientId)).Select(user => user.GCMClientId),
                NotificationData = PushMessageComposer.GetMessage(evt, "EventEnd")
            });
        }

        public void NotifyExtendEvent(EventSlim evt, EventRequestSlim eventRequest)
        {
            _taskQueue.Enqueue(new
            {
                RegistrationIds = evt.UserList.Where(user =>
                !_isRequester(user.UserId, evt.InitiatorId.ToString()) &&
                _isValidGCMId(user.GCMClientId)).Select(user => user.GCMClientId),
                NotificationData = PushMessageComposer.GetMessageForExtend(evt, "EventExtend", eventRequest.ExtendEventDuration)
            });
        }

        public void NotifyDeleteEvent(EventSlim evt)
        {
            _taskQueue.Enqueue(new
            {
                RegistrationIds = evt.UserList.Where(user =>
                !_isRequester(user.UserId, evt.InitiatorId.ToString()) &&
                _isValidGCMId(user.GCMClientId)).Select(user => user.GCMClientId),
                NotificationData = PushMessageComposer.GetMessage(evt, "EventDeleted")
            });
        }

        public void NotifyLeaveEvent(EventSlim evt, EventRequestSlim eventRequest)
        {
            _taskQueue.Enqueue(new
            {
                RegistrationIds = evt.UserList.Where(user =>
                !_isRequester(user.UserId, evt.InitiatorId.ToString()) &&
                _isValidGCMId(user.GCMClientId)).Select(user => user.GCMClientId),
                NotificationData = PushMessageComposer.GetMessageForLeaveEvent(evt, eventRequest)
            });
        }

        public void NotifyAdditionalRegisteredUserInfoToHost(List<EventParticipantSlim> additionalRegisteredUsers, EventSlim evt)
        {
            var registrationIds = evt.UserList.Where(user =>
                        _isRequester(user.UserId, evt.InitiatorId.ToString()) &&
                        _isValidGCMId(user.GCMClientId)).Select(user => user.GCMClientId);

            additionalRegisteredUsers.ForEach(x =>
            {
                _taskQueue.Enqueue(new { RegistrationIds = registrationIds, NotificationData = PushMessageComposer.GetMessage("RegisteredUserUpdate", x.UserId, x.MobileNumberStoredInRequestorPhone) });
            });
        }

        public void NotifyEventResponse(EventRequestSlim eventRequest, EventSlim evt)
        {
            _taskQueue.Enqueue(new
            {
                RegistrationIds = evt.UserList.Where(user =>
                _isRequester(user.UserId, evt.InitiatorId.ToString()) &&
                _isValidGCMId(user.GCMClientId)).Select(user => user.GCMClientId),
                NotificationData = PushMessageComposer.GetMessage(eventRequest, evt, "EventResponse")
            });
        }
        public void NotifyRemindContact(RemindRequestSlim remindReq)
        {
            _taskQueue.Enqueue(new
            {
                RegistrationIds = remindReq.ParticipantsForReminder.Where(user =>
                !_isRequester(user.UserId, remindReq.RequestorId.ToString()) &&
                _isValidGCMId(user.GCMClientId)).Select(user => user.GCMClientId),
                NotificationData = PushMessageComposer.GetMessage(remindReq)
            });
        }
    }
}
