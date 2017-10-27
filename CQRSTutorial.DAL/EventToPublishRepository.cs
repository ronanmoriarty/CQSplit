using System;
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
            EventStoreContext.EventsToPublish.Add(eventToPublish);
        }

        private EventStoreContext EventStoreContext => ((EntityFrameworkUnitOfWork)UnitOfWork).EventStoreContext;

        public IEvent Read(Guid id)
        {
            var eventToPublish = EventStoreContext.EventsToPublish.SingleOrDefault(x => x.Id == id);
            if (eventToPublish == null)
            {
                return null;
            }

            return _eventToPublishMapper.MapToEvent(eventToPublish);
        }

        public EventsToPublishResult GetEventsAwaitingPublishing(int batchSize)
        {
            var eventsToPublish = EventStoreContext.EventsToPublish.OrderBy(x => x.Created).Take(batchSize);
            var totalNumberOfEventsToPublish = eventsToPublish.Count();

            return new EventsToPublishResult
            {
                EventsToPublish = eventsToPublish.ToList(),
                TotalNumberOfEventsToPublish = totalNumberOfEventsToPublish
            };
        }

        public void Delete(EventToPublish eventToPublish)
        {
            EventStoreContext.EventsToPublish.Remove(eventToPublish);
        }

        public IUnitOfWork UnitOfWork { get; set; }
    }
}
