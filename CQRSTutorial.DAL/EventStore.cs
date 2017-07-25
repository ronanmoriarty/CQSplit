using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CQRSTutorial.Core;
using Newtonsoft.Json;
using NHibernate;
using NHibernate.Criterion;

namespace CQRSTutorial.DAL
{
    public class EventStore : RepositoryBase<Event>, IEventStore
    {
        private readonly EventMapper _eventMapper;

        public EventStore(ISessionFactory readSessionFactory,
            EventMapper eventMapper)
            : base(readSessionFactory, IsolationLevel.ReadCommitted)
        {
            _eventMapper = eventMapper;
        }

        public void Add(IEvent @event)
        {
            var eventToStore = new Event
            {
                AggregateId = @event.AggregateId,
                EventType = @event.GetType().Name,
                Data = JsonConvert.SerializeObject(@event),
                Created = DateTime.Now
            };
            SaveOrUpdate(eventToStore);
            UpdateEventIdToReflectIdAssignedByNHibernateToEventToStore(@event, eventToStore);
        }

        public IEvent Read(int id)
        {
            var storedEvent = Get(id);
            return _eventMapper.MapToEvent(storedEvent);
        }

        public IEnumerable<IEvent> GetAllEventsFor(int aggregateId)
        {
            using (var session = ReadSessionFactory.OpenSession())
            {
                using (session.BeginTransaction(IsolationLevel))
                {
                    var criteria = session.CreateCriteria<Event>();
                    criteria.Add(Restrictions.Eq("AggregateId", aggregateId));
                    var events = (IEnumerable<Event>)criteria.List<Event>();
                    return events.Select(@event => _eventMapper.MapToEvent(@event)).ToList();
                }
            }
        }

        private void UpdateEventIdToReflectIdAssignedByNHibernateToEventToStore(IEvent @event, Event eventToStore)
        {
            // We're not saving the event itself - so the event's id doesn't get updated automatically by NHibernate. Only the event-to-store's Id gets updated during saving.
            @event.Id = eventToStore.Id;
        }
    }
}