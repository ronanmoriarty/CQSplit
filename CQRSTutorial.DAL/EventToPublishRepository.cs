using System.Collections.Generic;
using System.Data;
using CQRSTutorial.Core;
using Newtonsoft.Json;
using NHibernate;

namespace CQRSTutorial.DAL
{
    public class EventToPublishRepository : RepositoryBase<EventToPublish>, IEventRepository
    {
        private readonly IPublishConfiguration _publishConfiguration;
        private readonly EventToPublishMapper _eventToPublishMapper;

        public EventToPublishRepository(ISessionFactory readSessionFactory,
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

        public IList<EventToPublish> GetEventsAwaitingPublishing()
        {
            using (var session = ReadSessionFactory.OpenSession())
            {
                using (session.BeginTransaction(IsolationLevel))
                {
                    return session.QueryOver<EventToPublish>().List();
                }
            }
        }
    }
}
