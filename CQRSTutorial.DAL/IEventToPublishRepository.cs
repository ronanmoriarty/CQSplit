using System;
using System.Collections.Generic;
using CQRSTutorial.Core;

namespace CQRSTutorial.DAL
{
    public interface IEventToPublishRepository : IHaveUnitOfWork
    {
        void Add(IEvent @event);
        IEvent Read(Guid id);
        IList<EventToPublish> GetEventsAwaitingPublishing(int batchSize);
        void Delete(EventToPublish eventToPublish);
    }
}