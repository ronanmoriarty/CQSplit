using System;
using System.Collections.Generic;
using CQRSTutorial.Core;

namespace CQRSTutorial.DAL
{
    public interface IEventToPublishRepository : IEventStore
    {
        IEvent Read(Guid id);
        IList<EventToPublish> GetEventsAwaitingPublishing();
        void Delete(EventToPublish eventToPublish);
    }
}