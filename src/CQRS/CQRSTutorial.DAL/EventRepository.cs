using System;
using System.Collections.Generic;
using System.Linq;
using CQRSTutorial.Core;
using CQRSTutorial.DAL.Serialized;

namespace CQRSTutorial.DAL
{
    public class EventRepository : IEventRepository
    {
        private readonly EventSerializer _eventSerializer;

        public EventRepository(EventSerializer eventSerializer)
        {
            _eventSerializer = eventSerializer;
        }

        public void Add(IEvent @event)
        {
            var eventToStore = _eventSerializer.Serialize(@event);

            EventStoreDbContext.Events.Add(eventToStore);

            UpdateEventIdToReflectIdAssignedByORMToEventToStore(@event, eventToStore);
        }

        public EventStoreDbContext EventStoreDbContext => ((EventStoreUnitOfWork) UnitOfWork).EventStoreDbContext;

        public IEvent Read(Guid id)
        {
            var serializedEvent = EventStoreDbContext.Events.SingleOrDefault(x => x.Id == id);
            return _eventSerializer.Deserialize(serializedEvent);
        }

        public IEnumerable<IEvent> GetAllEventsFor(Guid aggregateId)
        {
            return EventStoreDbContext.Events.Select(@event => _eventSerializer.Deserialize(@event)).ToList();
        }

        private void UpdateEventIdToReflectIdAssignedByORMToEventToStore(IEvent @event, Event eventToStore)
        {
            // We're not saving the event itself - so the event's id doesn't get updated automatically by the ORM. Only the event-to-store's Id gets updated during saving.
            @event.Id = eventToStore.Id;
        }

        public IUnitOfWork UnitOfWork { get; set; }
    }
}