namespace Engaze.NotificationManager
{
    using Engaze.NotificationManager.Contract;
    using System.Collections.Generic;
    public interface INotificationManager
    {       
        void NotifyEventCreation(EventSlim evt);
        void NotifyEventUpdate(EventSlim evt, List<string> removedParticipantsGcms);
        void NotifyEventUpdateParticipants(EventSlim evt, List<string> removedParticipantsGcms);
        void NotifyEventUpdateLocation(EventRequestSlim eventRequest, EventSlim evt);
        void NotifyAddParticipantToEvent(EventSlim evt, List<string> adddedParticipantGcms);
        void NotifyRemoveParticipantFromEvent(EventSlim evt, List<string> removedParticipantsGcms);
        void NotifyEndEvent(EventSlim evt);
        void NotifyExtendEvent(EventSlim evt, EventRequestSlim eventRequest);
        void NotifyLeaveEvent(EventSlim evt, EventRequestSlim eventRequest);
        void NotifyDeleteEvent(EventSlim evt);
        void NotifyEventResponse(EventRequestSlim eventRequest, EventSlim evt);
        void NotifyAdditionalRegisteredUserInfoToHost(List<EventParticipantSlim> additionalRegisteredUsers, EventSlim evt);
        void NotifyRemindContact(RemindRequestSlim remindReq);
    }
}
