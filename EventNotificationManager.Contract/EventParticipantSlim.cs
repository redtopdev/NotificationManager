
namespace Engaze.NotificationManager.Contract
{
    using System;
    public class EventParticipantSlim
    {
        public string UserId { get; set; }  //Pass Guid for registered users. Pass empty/null for non registered
        public string ProfileName { get; set; }
        public string GCMClientId { get; set; }
        public string MobileNumberStoredInRequestorPhone { get; set; }

    }
}
