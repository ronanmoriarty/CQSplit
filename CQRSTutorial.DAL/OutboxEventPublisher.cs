using System.Collections.Generic;
using CQRSTutorial.Core;
using NHibernate;

namespace CQRSTutorial.DAL
{
    public class OutboxEventPublisher : IEventPublisher
    {
        private readonly ISessionFactory _writeSessionFactory;
        private readonly IEventRepository _eventRepository;

        public OutboxEventPublisher(ISessionFactory writeSessionFactory, IEventRepository eventRepository)
        {
            _writeSessionFactory = writeSessionFactory;
            _eventRepository = eventRepository;
        }

        public void Publish(IEnumerable<IEvent> events)
        {
            using (var writeSession = _writeSessionFactory.OpenSession())
            {
                _eventRepository.UnitOfWork = new NHibernateUnitOfWork(writeSession);
                using (var transaction = writeSession.BeginTransaction())
                {
                    try
                    {
                        foreach (var @event in events)
                        {
                            _eventRepository.Add(@event);
                        }
                        transaction.Commit();
                    }
                    catch (System.Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}
