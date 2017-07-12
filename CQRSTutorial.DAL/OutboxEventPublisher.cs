using System;
using System.Collections.Generic;
using CQRSTutorial.Core;
using NHibernate;

namespace CQRSTutorial.DAL
{
    public class OutboxEventPublisher : IEventPublisher
    {
        private readonly ISessionFactory _writeSessionFactory;
        private readonly IEventRepository _eventRepository;
        private readonly IEventRepository _eventStore;

        public OutboxEventPublisher(ISessionFactory writeSessionFactory, IEventRepository eventRepository, IEventRepository eventStore)
        {
            _writeSessionFactory = writeSessionFactory;
            _eventRepository = eventRepository;
            _eventStore = eventStore;
        }

        public void Publish(IEnumerable<IEvent> events)
        {
            using (var writeSession = _writeSessionFactory.OpenSession())
            {
                _eventRepository.UnitOfWork = new NHibernateUnitOfWork(writeSession); // TODO: we pass in an IEventRepository, but we're clearly working with NHibernate repositories. Remove references to NHibernate from this class.
                _eventStore.UnitOfWork = new NHibernateUnitOfWork(writeSession); // TODO: we pass in an IEventRepository, but we're clearly working with NHibernate repositories. Remove references to NHibernate from this class.
                using (var transaction = writeSession.BeginTransaction())
                {
                    try
                    {
                        foreach (var @event in events)
                        {
                            _eventStore.Add(@event);
                            _eventRepository.Add(@event);
                        }
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}
