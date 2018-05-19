namespace Engaze.NotificationManager
{
    using Engaze.NotificationManager.Contract;
    using System.Linq;
    public static class PushMessageComposer
    {
        public static  PushNotification GetMessage(EventSlim evt, string notificationType)
        {
            return new PushNotification()
            {
                EventId = evt.EventId.ToString(),
                EventName = evt.Name,
                Type = notificationType, //"EventInvite",
                InitiatorId = evt.InitiatorId.ToString(),
                InitiatorName = evt.InitiatorName
            };
        }

        //Used for event invite, event end
        public static PushNotification GetMessageForLeaveEvent(EventSlim evt, EventRequestSlim eventRequest)
        {
            return new PushNotification()
            {
                EventId = evt.EventId.ToString(),
                EventName = evt.Name,
                Type = "EventLeave",
                EventResponderId = eventRequest.RequestorId.ToString(),
                EventResponderName = evt.UserList.Where(x => x.UserId.ToString().ToLower() == eventRequest.RequestorId.ToString().ToLower()).ToList()[0].ProfileName.ToString()
            };
        }

        //Used for remind contact 
        public static PushNotification GetMessage(RemindRequest remindReq)
        {
            PushNotification pn = new PushNotification();

            if (remindReq.EventId != null)
            {
                pn.EventId = remindReq.EventId.ToString();
                if (!string.IsNullOrEmpty(remindReq.EventName))
                    pn.EventName = remindReq.EventName;
            }
            pn.Type = "RemindContact";
            pn.InitiatorId = remindReq.RequestorId;
            pn.InitiatorName = remindReq.RequestorName;
            return pn;
        }

        //Used for update event location
        public static PushNotification GetMessageForUpdateLocation(EventSlim evt, EventRequestSlim eventRequest, string notificationType)
        {
            return new PushNotification()
            {
                EventId = evt.EventId.ToString(),
                EventName = evt.Name,
                Type = notificationType,
                InitiatorId = evt.InitiatorId.ToString(),
                InitiatorName = evt.InitiatorName,
                DestinationName = eventRequest.DestinationName
            };
        }

        public static PushNotification GetMessageForExtend(EventSlim evt, string notificationType, int extendEventDuration)
        {
            return new PushNotification()
            {
                EventId = evt.EventId.ToString(),
                EventName = evt.Name,
                Type = notificationType, //"EventInvite",
                InitiatorId = evt.InitiatorId.ToString(),
                InitiatorName = evt.InitiatorName,
                ExtendEventDuration = extendEventDuration
            };
        }

        //Used for event acceptance
        public static PushNotification GetMessage(EventRequestSlim eventRequest, EventSlim evt, string notificationType)
        {
            return new PushNotification()
            {
                EventId = evt.EventId.ToString(),
                EventName = evt.Name,
                Type = notificationType, //"EventResponse",
                InitiatorId = evt.InitiatorId.ToString(),
                EventResponderId = eventRequest.RequestorId.ToString(),
                EventResponderName = evt.UserList.First(x => x.UserId.ToString().ToLower() == eventRequest.RequestorId.ToLower()).ProfileName,
                EventAcceptanceStateId = eventRequest.EventAcceptanceStateId,
                TrackingAccepted = eventRequest.TrackingAccepted
            };
        }
        public static PushNotification GetMessage(string notificationType, string userId, string mobileNumber)
        {
            return new PushNotification()
            {
                Type = notificationType,
                UserId = userId,
                MobileNumber = mobileNumber
            };
        }
    }
}
