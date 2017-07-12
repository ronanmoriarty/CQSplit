using System.Data;
using CQRSTutorial.Core;
using Newtonsoft.Json;
using NHibernate;

namespace CQRSTutorial.DAL
{
    public class EventRepository : RepositoryBase<EventToPublish>, IEventRepository
    {
        private readonly IPublishConfiguration _publishConfiguration;
        private readonly EventToPublishMapper _eventToPublishMapper;

        public EventRepository(ISessionFactory readSessionFactory,
            IsolationLevel isolationLevel,
            IPublishConfiguration publishConfiguration,
            EventToPublishMapper eventToPublishMapper)
            : base(readSessionFactory, isolationLevel)
        {
            _publishConfiguration = publishConfiguration;
            _eventToPublishMapper = eventToPublishMapper;
        }

        public void Add(IEvent @event)
        {
            var eventToPublish = new EventToPublish
            {
                EventType = @event.GetType().Name,
                Data = JsonConvert.SerializeObject(@event),
                PublishTo = _publishConfiguration.GetPublishLocationFor(@event.GetType())
            };
            SaveOrUpdate(eventToPublish);
            UpdateEventIdToReflectIdAssignedByNHibernateToEventToPublish(@event, eventToPublish);
        }

        public IEvent Read(int id)
        {
            var eventToPublish = Get(id);
            return _eventToPublishMapper.MapToEvent(eventToPublish);
        }

        private void UpdateEventIdToReflectIdAssignedByNHibernateToEventToPublish(IEvent @event,
            EventToPublish eventToPublish)
        {
            // We're not saving the event itself - so the event's id doesn't get updated automatically by NHibernate. Only the EventToPublish's Id gets updated during saving.
            @event.Id = eventToPublish.Id;
        }
    }
}
