using System;
using System.Collections.Generic;

namespace Engaze.NotificationManager.Contract
{
    public class EventSlim
    {
        public Guid EventId { get; set; }
        public string Name { get; set; }       
        public List<EventParticipantSlim> UserList { get; set; }
        public Guid InitiatorId { get; set; }
        public string InitiatorName { get; set; }
        public string InitiatorGCMClientId { get; set; }
        public string Description { get; set; }
    }
}
