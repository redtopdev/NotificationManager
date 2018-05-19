using Engaze.NotificationManager.Contract;
using System.Collections.Generic;


namespace Engaze.NotificationManager.PNM
{
    public interface IPushNotifier
    {
        void Invite(EventSlim evnt, List<UserProfile> users);
        void SendInvite(List<string> registrationIds, PushNotification notificationData);
    }
}
