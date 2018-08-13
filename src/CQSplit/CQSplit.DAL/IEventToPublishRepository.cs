using System;
using System.Collections.Generic;
using CQSplit.DAL.Serialized;

namespace CQSplit.DAL
{
    public interface IEventToPublishRepository : IEventStore
    {
        EventToPublish Read(Guid id);
        IList<EventToPublish> GetEventsAwaitingPublishing();
        void Delete(EventToPublish eventToPublish);
    }
}