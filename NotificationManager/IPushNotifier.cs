using Engaze.NotificationManager.Contract;
using System.Collections.Generic;


namespace Engaze.NotificationManager
{
    public interface IPushNotifier
    {
        void PushNotification(ICollection<string> registrationIds, PushNotification notificationData);
    }
}
