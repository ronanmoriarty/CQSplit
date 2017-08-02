using System;
using System.Collections.Generic;
using CQRSTutorial.Core;
using NHibernate;

namespace CQRSTutorial.DAL
{
    public class EventToPublishRepository : RepositoryBase<EventToPublish>, IEventToPublishRepository
    {
        private readonly EventToPublishMapper _eventToPublishMapper;

        public EventToPublishRepository(ISessionFactory sessionFactory,
            EventToPublishMapper eventToPublishMapper)
            : base(sessionFactory)
        {
            _eventToPublishMapper = eventToPublishMapper;
        }

        public void Add(IEvent @event)
        {
            var eventToPublish = _eventToPublishMapper.MapToEventToPublish(@event);
            SaveOrUpdate(eventToPublish);
        }

        public IEvent Read(Guid id)
        {
            var eventToPublish = Get(id);
            return _eventToPublishMapper.MapToEvent(eventToPublish);
        }

        public EventsToPublishResult GetEventsAwaitingPublishing(int batchSize)
        {
            using (var session = SessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    var eventsToPublish = session
                        .QueryOver<EventToPublish>()
                        .OrderBy(x => x.Created).Asc
                        .Take(batchSize)
                        .List();

                    return new EventsToPublishResult
                    {
                        EventsToPublish = eventsToPublish
                    };
                }
            }
        }
    }
}
