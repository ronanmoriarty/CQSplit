using System;
using System.Collections.Generic;
using CQRSTutorial.Core;
using Newtonsoft.Json;
using NHibernate;

namespace CQRSTutorial.DAL
{
    public class EventToPublishRepository : RepositoryBase<EventToPublish>, IEventRepository
    {
        private readonly IPublishConfiguration _publishConfiguration;
        private readonly EventToPublishMapper _eventToPublishMapper;

        public EventToPublishRepository(ISessionFactory sessionFactory,
            IPublishConfiguration publishConfiguration,
            EventToPublishMapper eventToPublishMapper)
            : base(sessionFactory)
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

        public IEvent Read(Guid id)
        {
            var eventToPublish = Get(id);
            return _eventToPublishMapper.MapToEvent(eventToPublish);
        }

        public IList<EventToPublish> GetEventsAwaitingPublishing()
        {
            using (var session = SessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    return session.QueryOver<EventToPublish>().List();
                }
            }
        }
    }
}
