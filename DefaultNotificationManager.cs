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
        private Predicate<string> _isNotValidGCMId;

        public DefaultNotificationManager(IPushNotifier pushNotifier, INotificationTaskQueue taskQueue)
        {
            _pushNotifier = new GCMNotifier();
            _taskQueue = taskQueue;
            _isNotValidGCMId = Id => Id.ToUpper().CompareTo("TEMP") == 0;

        }

        public Boolean NotifyEventCreation(EventSlim evt)
        {
            try
            {
                List<string> registrationIds = new List<string>();
                evt.UserList.ForEach(user =>
                    {
                        if (!user.UserId.ToString().ToLower().Equals(evt.InitiatorId.ToString().ToLower()))
                        {
                            registrationIds.Add(user.GCMClientId);
                        }
                    });

                _taskQueue.Enqueue(new { RegistrationIds = GetValidGcmIds(registrationIds), NotificationData = PushMessageComposer.GetMessage(evt, "EventInvite" });

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Boolean NotifyEventUpdate(EventSlim evt, List<string> removedParticipantsGcms)
        {
            try
            {
                if (removedParticipantsGcms == null)
                {
                    removedParticipantsGcms = new List<string>();
                }

                List<string> registrationIds = new List<string>();
                evt.UserList.ForEach(user =>
                {
                    if (!user.UserId.ToString().ToLower().Equals(evt.InitiatorId.ToString().ToLower()) && (!removedParticipantsGcms.Contains(user.GCMClientId))) // && (user.EventAcceptanceStateId != 0))                       
                    {
                        registrationIds.Add(user.GCMClientId);
                    }
                });

                _taskQueue.Enqueue(new { RegistrationIds = GetValidGcmIds(registrationIds), NotificationData = PushMessageComposer.GetMessage(evt, "EventUpdate") });

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Boolean NotifyEventUpdateParticipants(EventSlim evt, List<string> removedParticipantsGcms)
        {
            try
            {
                if (removedParticipantsGcms == null)
                {
                    removedParticipantsGcms = new List<string>();
                }

                List<string> registrationIds = new List<string>();
                evt.UserList.ForEach(user =>
                {
                    if (!user.UserId.ToString().ToLower().Equals(evt.InitiatorId.ToString().ToLower()) && (!removedParticipantsGcms.Contains(user.GCMClientId))) // && (user.EventAcceptanceStateId != 0))
                    {
                        registrationIds.Add(user.GCMClientId);
                    }
                });


                _taskQueue.Enqueue(new { RegistrationIds = GetValidGcmIds(registrationIds), NotificationData = PushMessageComposer.GetMessage(evt, "EventUpdateParticipants") });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Boolean NotifyEventUpdateLocation(EventRequestSlim eventRequest, EventSlim evt)
        {
            try
            {
                List<string> registrationIds = new List<string>();
                evt.UserList.ForEach(user =>
                {
                    if (!user.UserId.ToString().ToLower().Equals(evt.InitiatorId.ToString().ToLower())) // && (user.EventAcceptanceStateId != 0))
                    {
                        registrationIds.Add(user.GCMClientId);
                    }
                });

                _taskQueue.Enqueue(new { RegistrationIds = GetValidGcmIds(registrationIds), NotificationData = PushMessageComposer.GetMessageForUpdateLocation(evt, eventRequest, "EventUpdateLocation") });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Boolean NotifyAddParticipantToEvent(EventSlim evt, List<string> adddedParticipantGcms)
        {
            try
            {
                //PushNotification pn = GetMessage(evt, "EventUpdate");
                if (adddedParticipantGcms.Count == 0)
                { return true; }

                //Notify newly added users of event invitation
                _taskQueue.Enqueue(new { RegistrationIds = GetValidGcmIds(adddedParticipantGcms), NotificationData = PushMessageComposer.GetMessage(evt, "EventInvite") });

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Boolean NotifyRemoveParticipantFromEvent(EventSlim evt, List<string> removedParticipantGcms)
        {
            try
            {
                if (removedParticipantGcms.Count == 0)
                {
                    return true;
                }
                _taskQueue.Enqueue(new { RegistrationIds = GetValidGcmIds(removedParticipantGcms), NotificationData = PushMessageComposer.GetMessage(evt, "RemovedFromEvent") });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Boolean NotifyEndEvent(EventSlim evt)
        {
            try
            {
                List<string> registrationIds = new List<string>();
                evt.UserList.ForEach(user =>
                {
                    if (!user.UserId.ToString().ToLower().Equals(evt.InitiatorId.ToString().ToLower())) // && (user.EventAcceptanceStateId != 0))
                    {
                        registrationIds.Add(user.GCMClientId);
                    }
                });

                _taskQueue.Enqueue(new { RegistrationIds = GetValidGcmIds(registrationIds), NotificationData = PushMessageComposer.GetMessage(evt, "EventEnd") });

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Boolean NotifyExtendEvent(EventSlim evt, EventRequestSlim eventRequest)
        {
            try
            {

                List<string> registrationIds = new List<string>();
                evt.UserList.ForEach(user =>
                {
                    if (!user.UserId.ToString().ToLower().Equals(evt.InitiatorId.ToString().ToLower())) //  && (user.EventAcceptanceStateId != 0))
                    {
                        registrationIds.Add(user.GCMClientId);
                    }
                });

                _taskQueue.Enqueue(new { RegistrationIds = GetValidGcmIds(registrationIds), NotificationData = PushMessageComposer.GetMessageForExtend(evt, "EventExtend", eventRequest.ExtendEventDuration) });

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Boolean NotifyDeleteEvent(EventSlim evt)
        {
            try
            {
                List<string> registrationIds = new List<string>();
                evt.UserList.ForEach(user =>
                {
                    if (!user.UserId.ToString().ToLower().Equals(evt.InitiatorId.ToString().ToLower()))
                    {
                        registrationIds.Add(user.GCMClientId);
                    }
                });

                _taskQueue.Enqueue(new { RegistrationIds = GetValidGcmIds(registrationIds), NotificationData = PushMessageComposer.GetMessage(evt, "EventDeleted") });

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Boolean NotifyLeaveEvent(EventSlim evt, EventRequestSlim eventRequest)
        {
            try
            {
                List<string> registrationIds = new List<string>();
                evt.UserList.ForEach(user =>
                {
                    if (!user.UserId.ToString().ToLower().Equals(eventRequest.RequestorId.ToString().ToLower())) // && (user.EventAcceptanceStateId != 0))
                    {
                        registrationIds.Add(user.GCMClientId);
                    }
                });

                _taskQueue.Enqueue(new { RegistrationIds = GetValidGcmIds(registrationIds), NotificationData = PushMessageComposer.GetMessageForLeaveEvent(evt, eventRequest) });

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Boolean NotifyAdditionalRegisteredUserInfoToHost(List<EventParticipantSlim> additionalRegisteredUsers, EventSlim evt)
        {
            try
            {
                List<string> registrationIds = new List<string>();
                registrationIds.Add(evt.InitiatorGCMClientId.ToString());
                registrationIds = GetValidGcmIds(registrationIds);

                additionalRegisteredUsers.ForEach(x =>
                {
                    _taskQueue.Enqueue(new { RegistrationIds = registrationIds, NotificationData = PushMessageComposer.GetMessage("RegisteredUserUpdate", x.UserId, x.MobileNumberStoredInRequestorPhone) });
                });

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Boolean NotifyEventResponse(EventRequestSlim eventRequest, EventSlim evt)
        {
            try
            {
                var registrationIds = evt.UserList.Except(evt.UserList
                    .Where(user => user.UserId.ToLower().Equals(eventRequest.RequestorId.ToLower())//remove requester
                    || _isNotValidGCMId(user.UserId)))//remove invalid
                    .Select(user => user.GCMClientId);

                _taskQueue.Enqueue(new { RegistrationIds = registrationIds, NotificationData = PushMessageComposer.GetMessage(eventRequest, evt, "EventResponse") });

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Boolean NotifyRemindContact(RemindRequest remindReq)
        {
            try
            {
                List<string> registrationIds = new List<string>();
                remindReq.ContactNumbersForRemindFormatted.ForEach(contact =>
                    {
                        if (!string.IsNullOrEmpty(contact.UserId))
                        {
                            if (!contact.UserId.ToString().ToLower().Equals(remindReq.RequestorId.ToString().ToLower()))
                                registrationIds.Add(contact.GCMClientId);
                        }
                        else
                        {
                            registrationIds.Add(contact.GCMClientId);
                        }
                    });

                registrationIds = GetValidGcmIds(registrationIds);
                _taskQueue.Enqueue(new { RegistrationIds = registrationIds, NotificationData = PushMessageComposer.GetMessage(remindReq) });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        List<string> GetValidGcmIds(IEnumerable<string> registrationIds)
        {
            if (registrationIds != null)
            {
                return registrationIds.Except(registrationIds.Where(id => _isNotValidGCMId(id))).ToList();
            }
            return null;
        }
    }
}
