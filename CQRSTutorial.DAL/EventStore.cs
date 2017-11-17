using System;
using System.Collections.Generic;
using System.Linq;
using CQRSTutorial.Core;
using Newtonsoft.Json;

namespace CQRSTutorial.DAL
{
    public class EventStore : IEventRepository
    {
        private readonly EventSerializer _eventSerializer;

        public EventStore(EventSerializer eventSerializer)
        {
            _eventSerializer = eventSerializer;
        }

        public void Add(IEvent @event)
        {
            var eventToStore = new Event
            {
                Id = @event.Id,
                AggregateId = @event.AggregateId,
                CommandId = @event.CommandId,
                EventType = @event.GetType().Name,
                Data = JsonConvert.SerializeObject(@event),
                Created = DateTime.Now
            };

            EventStoreDbContext.Events.Add(eventToStore);

            UpdateEventIdToReflectIdAssignedByORMToEventToStore(@event, eventToStore);
        }

        public EventStoreDbContext EventStoreDbContext => ((EventStoreUnitOfWork) UnitOfWork).EventStoreDbContext;

        public IEvent Read(Guid id)
        {
            var storedEvent = Get(id);
            return _eventSerializer.Deserialize(storedEvent);
        }

        private Event Get(Guid id)
        {
            return EventStoreDbContext.Events.SingleOrDefault(x => x.Id == id);
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