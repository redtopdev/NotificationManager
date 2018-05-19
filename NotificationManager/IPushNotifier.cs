using Engaze.NotificationManager.Contract;
using System.Collections.Generic;


namespace Engaze.NotificationManager
{
    public interface IPushNotifier
    {
        void Invite(EventSlim evnt, ICollection<string> gcmClientIds);
        void SendInvite(List<string> registrationIds, PushNotification notificationData);
    }
}
