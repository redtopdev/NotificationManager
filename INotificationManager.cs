namespace Engaze.NotificationManager
{
    using Engaze.NotificationManager.Contract;
    using System;
    using System.Collections.Generic;
    public interface INotificationManager
    {       
        Boolean NotifyEventCreation(EventSlim evt);
        Boolean NotifyEventUpdate(EventSlim evt, List<string> removedParticipantsGcms);
        Boolean NotifyEventUpdateParticipants(EventSlim evt, List<string> removedParticipantsGcms);
        Boolean NotifyEventUpdateLocation(EventRequestSlim eventRequest, EventSlim evt);
        Boolean NotifyAddParticipantToEvent(EventSlim evt, List<string> adddedParticipantGcms);
        Boolean NotifyRemoveParticipantFromEvent(EventSlim evt, List<string> removedParticipantsGcms);
        Boolean NotifyEndEvent(EventSlim evt);
        Boolean NotifyExtendEvent(EventSlim evt, EventRequestSlim eventRequest);
        Boolean NotifyLeaveEvent(EventSlim evt, EventRequestSlim eventRequest);
        Boolean NotifyDeleteEvent(EventSlim evt);
        Boolean NotifyEventResponse(EventRequestSlim eventRequest, EventSlim evt);
        Boolean NotifyAdditionalRegisteredUserInfoToHost(List<EventParticipantSlim> additionalRegisteredUsers, EventSlim evt);
        Boolean NotifyRemindContact(RemindRequest remindReq);
    }
}
