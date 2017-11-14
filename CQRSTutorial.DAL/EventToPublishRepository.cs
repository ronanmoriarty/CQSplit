using System;
using System.Collections.Generic;
using System.Linq;
using CQRSTutorial.Core;

namespace CQRSTutorial.DAL
{
    public class EventToPublishRepository : IEventToPublishRepository
    {
        private readonly EventToPublishSerializer _eventToPublishSerializer;

        public EventToPublishRepository(EventToPublishSerializer eventToPublishSerializer)
        {
            _eventToPublishSerializer = eventToPublishSerializer;
        }

        public void Add(IEvent @event)
        {
            var eventToPublish = _eventToPublishSerializer.Serialize(@event);
            EventStoreDbContext.EventsToPublish.Add(eventToPublish);
        }

        private EventStoreDbContext EventStoreDbContext => ((EventStoreUnitOfWork)UnitOfWork).EventStoreDbContext;

        public EventToPublish Read(Guid id)
        {
            return EventStoreDbContext.EventsToPublish.SingleOrDefault(x => x.Id == id);
        }

        public IList<EventToPublish> GetEventsAwaitingPublishing()
        {
            return EventStoreDbContext
                .EventsToPublish
                .OrderBy(x => x.Created)
                .ToList();
        }

        public void Delete(EventToPublish eventToPublish)
        {
            EventStoreDbContext.EventsToPublish.Remove(eventToPublish);
        }

        public IUnitOfWork UnitOfWork { get; set; }
    }
}
