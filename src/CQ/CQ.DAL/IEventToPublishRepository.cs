using System;
using System.Collections.Generic;
using CQ.DAL.Serialized;

namespace CQ.DAL
{
    public interface IEventToPublishRepository : IEventStore
    {
        EventToPublish Read(Guid id);
        IList<EventToPublish> GetEventsAwaitingPublishing();
        void Delete(EventToPublish eventToPublish);
    }
}