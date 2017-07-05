using System;
using CQRSTutorial.Core;
using NHibernate;

namespace CQRSTutorial.DAL.Tests
{
    public class EventRepositoryDecorator : IEventRepository
    {
        private readonly IEventRepository _eventRepository;

        public EventRepositoryDecorator(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public void Add(IEvent @event)
        {
            // This is all a bit smoke-and-mirrors, but I can't think of a better way to do this without having to change to the OutboxEventPublisher / EventRepository
            // *specifically* for testing purposes, which always feels wrong. At least this way, the extra complication is contained within the tests, and not the 
            // system-under-test.
            OnBeforeAdding(@event);
            _eventRepository.Add(@event);
            OnAfterAdding(@event);
        }

        public Action<IEvent> OnBeforeAdding { get; set; } = @event => { };
        public Action<IEvent> OnAfterAdding { get; set; } = @event => { };

        public IEvent Read(Guid id)
        {
            return _eventRepository.Read(id);
        }

        public object UnitOfWork
        {
            get { return _eventRepository.UnitOfWork; }
            set { _eventRepository.UnitOfWork = value; }
        }
    }
}