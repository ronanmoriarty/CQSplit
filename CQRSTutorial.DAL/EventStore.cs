using System.Data;
using CQRSTutorial.Core;
using Newtonsoft.Json;
using NHibernate;

namespace CQRSTutorial.DAL
{
    public class EventStore : RepositoryBase<Event>, IEventStore
    {
        private readonly EventMapper _eventMapper;

        public EventStore(ISessionFactory readSessionFactory,
            IsolationLevel isolationLevel,
            EventMapper eventMapper)
            : base(readSessionFactory, isolationLevel)
        {
            _eventMapper = eventMapper;
        }

        public void Add(IEvent @event)
        {
            var eventToStore = new Event
            {
                EventType = @event.GetType().Name,
                Data = JsonConvert.SerializeObject(@event)
            };
            SaveOrUpdate(eventToStore);
            UpdateEventIdToReflectIdAssignedByNHibernateToEventToStore(@event, eventToStore);
        }

        public IEvent Read(int id)
        {
            var storedEvent = Get(id);
            return _eventMapper.MapToEvent(storedEvent);
        }

        private void UpdateEventIdToReflectIdAssignedByNHibernateToEventToStore(IEvent @event, Event eventToStore)
        {
            // We're not saving the event itself - so the event's id doesn't get updated automatically by NHibernate. Only the event-to-store's Id gets updated during saving.
            @event.Id = eventToStore.Id;
        }
    }
}