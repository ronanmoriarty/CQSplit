using System.Collections.Generic;

namespace CQRSTutorial.DAL
{
    public class EventsToPublishResult
    {
        public int TotalNumberOfEventsToPublish { get; set; }
        public IList<EventToPublish> EventsToPublish { get; set; }
    }
}