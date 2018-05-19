using System;
using System.Collections.Generic;
using System.Text;

namespace Engaze.NotificationManager.Contract
{
    public class RemindRequestSlim
    {
        public string EventName { get; set; }
        public string EventId { get; set; }
        public string RequestorId { get; set; }
        public string RequestorName { get; set; }        
        public List<EventParticipantSlim> ParticipantsForReminder{ get; set; }

    }
}
