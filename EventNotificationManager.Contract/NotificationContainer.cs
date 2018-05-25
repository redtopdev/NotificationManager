using System.Collections.Generic;

namespace Engaze.NotificationManager.Contract
{
    public class NotificationContainer
    {
        public List<string> RegistrationIds {get; set;}
        public PushNotification NotificationData { get; set; }
    }
}
