using System;
using System.Data;
using System.Linq;
using Cafe.Domain.Events;
using CQRSTutorial.Core;
using Newtonsoft.Json;
using NHibernate;

namespace CQRSTutorial.DAL
{
    public class EventRepository : RepositoryBase<EventDescriptor>, IEventRepository
    {
        private readonly IPublishConfiguration _publishConfiguration;

        public EventRepository(ISessionFactory readSessionFactory, IsolationLevel isolationLevel, IPublishConfiguration publishConfiguration)
            : base(readSessionFactory, isolationLevel)
        {
            _publishConfiguration = publishConfiguration;
        }

        public void Add(IEvent @event)
        {
            var eventDescriptor = new EventDescriptor
            {
                EventType = @event.GetType().Name,
                Data = JsonConvert.SerializeObject(@event),
                PublishTo = _publishConfiguration.GetPublishLocationFor(@event.GetType())
            };
            SaveOrUpdate(eventDescriptor);
            UpdateEventIdToReflectIdAssignedByNHibernateToEventDescriptor(@event, eventDescriptor);
        }

        public IEvent Read(int id)
        {
            var eventDescriptor = Get(id);
            var @event = (IEvent)JsonConvert.DeserializeObject(eventDescriptor.Data, GetEventTypeFrom(eventDescriptor.EventType));
            AssignEventIdFromEventDescriptorId(@event, eventDescriptor);
            return @event;
        }

        public EventDescriptor ReadEventDescriptor(int id)
        {
            return Get(id);
        }

        private Type GetEventTypeFrom(string eventType)
        {
            return typeof(TabOpened).Assembly.GetTypes()
                .Single(t => typeof(ITabEvent).IsAssignableFrom(t) && t.Name == eventType);
        }

        private void UpdateEventIdToReflectIdAssignedByNHibernateToEventDescriptor(IEvent @event,
            EventDescriptor eventDescriptor)
        {
            // We're not saving the event itself - so the event's id doesn't get updated automatically by NHibernate. Only the EventDescriptor's Id gets updated during saving.
            @event.Id = eventDescriptor.Id;
        }

        private void AssignEventIdFromEventDescriptorId(IEvent @event, EventDescriptor eventDescriptor)
        {
            @event.Id = eventDescriptor.Id;
        }
    }
}
