using System;
using System.Collections.Generic;

namespace CQSplit.Serialization
{
    public interface IEventToPublishRepository : IEventStore
    {
        EventToPublish Read(Guid id);
        IList<EventToPublish> GetEventsAwaitingPublishing();
        void Delete(EventToPublish eventToPublish);
    }
}