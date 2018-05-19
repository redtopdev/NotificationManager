namespace Engaze.NotificationManager.Contract
{
    using System;

    /// <summary>
    ///  This class is used for holding the request  object for getting Event list, Accepting event etc
    /// </summary>
    public class EventRequestSlim
    {
        public string RequestorId { get; set; }        
        public int EventAcceptanceStateId { get; set; }     
        public Boolean TrackingAccepted { get; set; }
        public int ExtendEventDuration { get; set; }        
        public string DestinationName { get; set; }
       
    }
}
