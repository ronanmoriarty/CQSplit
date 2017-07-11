using System.Data;
using CQRSTutorial.Core;
using Newtonsoft.Json;
using NHibernate;

namespace CQRSTutorial.DAL
{
    public class EventRepository : RepositoryBase<EventDescriptor>, IEventRepository
    {
        private readonly IPublishConfiguration _publishConfiguration;
        private readonly EventDescriptorMapper _eventDescriptorMapper;

        public EventRepository(ISessionFactory readSessionFactory,
            IsolationLevel isolationLevel,
            IPublishConfiguration publishConfiguration,
            EventDescriptorMapper eventDescriptorMapper)
            : base(readSessionFactory, isolationLevel)
        {
            _publishConfiguration = publishConfiguration;
            _eventDescriptorMapper = eventDescriptorMapper;
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
            return _eventDescriptorMapper.MapEventDescriptorToEvent(eventDescriptor);
        }

        private void UpdateEventIdToReflectIdAssignedByNHibernateToEventDescriptor(IEvent @event,
            EventDescriptor eventDescriptor)
        {
            // We're not saving the event itself - so the event's id doesn't get updated automatically by NHibernate. Only the EventDescriptor's Id gets updated during saving.
            @event.Id = eventDescriptor.Id;
        }
    }
}
