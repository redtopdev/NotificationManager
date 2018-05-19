using System.Threading;


namespace Engaze.NotificationManager
{
    public class NotificationTaskManager : NotificationTaskQueue
    {
        IPushNotifier _notifier;
        public NotificationTaskManager(IPushNotifier notifier)           
        {
            this._notifier = notifier;
        }
        protected override void Task(dynamic userData)
        {
            Thread.Sleep(1000);
            this._notifier.SendInvite(userData.RegistrationIds, userData.NotificationData);
        }    
    }
}
