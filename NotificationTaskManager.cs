using Engaze.NotificationManager.Contract;
using Engaze.NotificationManager.PNM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using Watchus.GCMServer;
using WatchUs.Interface.PushNotification;
using WatchUs.Model;


namespace Engaze.NotificationManager
{
    public class NotificationTaskManager : NotificationTaskQueue
    {
        IPushNotifier notifier;
        public NotificationTaskManager()           
        {
            this.notifier = new GCMNotifier();
        }
        protected override void Task(dynamic userData)
        {
            Thread.Sleep(1000);
            //Console.WriteLine(UserData.ToString());
            this.notifier.SendInvite(userData.RegistrationIds, userData.NotificationData);
        }
    
    }
}
