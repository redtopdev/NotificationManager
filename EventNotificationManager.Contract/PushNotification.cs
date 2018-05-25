
namespace Engaze.NotificationManager.Contract
{
    public class PushNotification
    {
        public string Type { get; set; }
        public string EventId { get; set; }
        public string EventName { get; set; }
        public string InitiatorId { get; set; }
        public string InitiatorName { get; set; }
        public string EventResponderId { get; set; }
        public string EventResponderName { get; set; }
        public int EventAcceptanceStateId { get; set; }
        public int ExtendEventDuration { get; set; }
        public bool TrackingAccepted { get; set; }
        public string UserId { get; set; }
        public string MobileNumber { get; set; }
        public string DestinationName { get; set; } 
    }
}
