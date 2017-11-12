using System;
using System.Collections.Generic;
using System.Linq;
using CQRSTutorial.Core;

namespace CQRSTutorial.DAL
{
    public class EventToPublishRepository : IEventToPublishRepository
    {
        private readonly EventToPublishMapper _eventToPublishMapper;

        public EventToPublishRepository(EventToPublishMapper eventToPublishMapper)
        {
            _eventToPublishMapper = eventToPublishMapper;
        }

        public void Add(IEvent @event)
        {
            var eventToPublish = _eventToPublishMapper.MapToEventToPublish(@event);
            EventStoreDbContext.EventsToPublish.Add(eventToPublish);
        }

        private EventStoreDbContext EventStoreDbContext => ((EventStoreUnitOfWork)UnitOfWork).EventStoreDbContext;

        public IEvent Read(Guid id)
        {
            var eventToPublish = EventStoreDbContext.EventsToPublish.SingleOrDefault(x => x.Id == id);
            if (eventToPublish == null)
            {
                return null;
            }

            return _eventToPublishMapper.MapToEvent(eventToPublish);
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
