using System;
using System.Collections.Generic;
using CQRSTutorial.DAL.Serialized;

namespace CQRSTutorial.DAL
{
    public interface IEventToPublishRepository : IEventStore
    {
        EventToPublish Read(Guid id);
        IList<EventToPublish> GetEventsAwaitingPublishing();
        void Delete(EventToPublish eventToPublish);
    }
}