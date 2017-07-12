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
                Id = @event.Id,
                EventType = @event.GetType().Name,
                Data = JsonConvert.SerializeObject(@event),
                PublishTo = _publishConfiguration.GetPublishLocationFor(@event.GetType())
            };
            SaveOrUpdate(eventToPublish);
        }

        public IEvent Read(int id)
        {
            var eventToPublish = Get(id);
            return _eventToPublishMapper.MapToEvent(eventToPublish);
        }
    }
}
