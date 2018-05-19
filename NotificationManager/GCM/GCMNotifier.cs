using System;
using System.Collections.Generic;
using Engaze.NotificationManager.Contract;
using PushSharp.Google;

namespace Engaze.NotificationManager.GCM
{
    public class GCMNotifier : IPushNotifier
    {
        //Android push message to GCM server method
        #region IPushNotifier Implemented methods

        public void Invite(EventSlim evnt, ICollection<string> gcmClientIds)
        {
            List<string> registrationIds = new List<string>();           
            GcmNotification notification = new GcmNotification();
            notification.ForDeviceRegistrationId(gcmClientIds);
            PushNotification notificationData = new PushNotification()
            {
                Type = "EventInvite",
                EventId = evnt.EventId.ToString(),
                EventName = evnt.Description
            };

            notification.WithJson(Serializer.SerializeToJason<PushNotification>(notificationData));

            this.PushNotification(notification);
        }



        public void SendInvite(List<string> registrationIds, PushNotification notificationData)
        {
            GcmNotification notification = new GcmNotification();
            notification.ForDeviceRegistrationId(registrationIds);

            notification.WithJson(Serializer.SerializeToJason<PushNotification>(notificationData));

            this.PushNotification(notification);
        }


        #endregion IPushNotifier Implemented methods


        #region private methods
        private void PushNotification(GcmNotification notification)
        {
            //Create our push services broker
            var push = new PushBroker();

            //Wire up the events for all the services that the broker registers
            push.OnNotificationSent += NotificationSent;
            push.OnChannelException += ChannelException;
            push.OnServiceException += ServiceException;
            push.OnNotificationFailed += NotificationFailed;
            push.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
            push.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
            push.OnChannelCreated += ChannelCreated;
            push.OnChannelDestroyed += ChannelDestroyed;

            push.RegisterGcmService(new GcmPushChannelSettings(ConfigurationManager.AppSettings["GCMAPIKey"]));
            //push.QueueNotification(new GcmNotification().ForDeviceRegistrationId("eWy5OUaxHJY:APA91bFNfAUJN7bt7HREMjgFQ653P_Q9vsgEizLStLx4TxcTrety3W-M0RgcB1plmu8C4SLbzwZOFHiCAyWXEjaQ0yzYB7m34yua-c78nsh32rY1e6aZVphrM1HAmw_NnfoxqvZeIuOk")
                                  //.WithJson(@"{""alert"":""Hello World!"",""badge"":7,""sound"":""sound.caf"",""msg"":""hello""}"));

            push.QueueNotification(notification);

            //Stop and wait for the queues to drains
            push.StopAllServices();
        }


        #region callbacks
        private void ChannelDestroyed(object sender)
        {
            //logging
        }

        private void ChannelCreated(object sender, IPushChannel pushChannel)
        {
            //logging
        }

        private void DeviceSubscriptionChanged(object sender, string oldSubscriptionId, string newSubscriptionId, INotification notification)
        {
            //logging
        }

        private void DeviceSubscriptionExpired(object sender, string expiredSubscriptionId, DateTime expirationDateUtc, INotification notification)
        {
            //logging
        }

        private void NotificationFailed(object sender, INotification notification, Exception error)
        {
            //logging
            
        }

        private void ServiceException(object sender, Exception error)
        {
            //logging
        }

        private void ChannelException(object sender, PushSharp.Core.IPushChannel pushChannel, Exception error)
        {
            //logging
        }

        private void NotificationSent(object sender, PushSharp.Core.INotification notification)
        {
            //logging
        }
        #endregion callbacks

        #endregion private methods
    }
}
