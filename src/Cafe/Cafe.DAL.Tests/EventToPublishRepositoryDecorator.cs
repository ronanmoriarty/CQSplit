using System;
using System.Collections.Generic;
using Cafe.DAL.Sql;
using CQSplit.Core;
using CQSplit.DAL;
using CQSplit.DAL.Serialized;

namespace Cafe.DAL.Tests
{
    public class EventToPublishRepositoryDecorator : IEventToPublishRepository
    {
        private readonly EventToPublishRepository _eventToPublishRepository;

        public EventToPublishRepositoryDecorator(EventToPublishRepository eventToPublishRepository)
        {
            _eventToPublishRepository = eventToPublishRepository;
        }

        public void Add(IEvent @event)
        {
            // This is all a bit smoke-and-mirrors, but I can't think of a better way to do this without having to change to the OutboxEventPublisher / EventToPublishRepository
            // *specifically* for testing purposes, which always feels wrong. At least this way, the extra complication is contained within the tests, and not the
            // system-under-test.
            OnBeforeAdding(@event);
            _eventToPublishRepository.Add(@event);
            OnAfterAdding(@event);
        }

        public Action<IEvent> OnBeforeAdding { get; set; } = @event => { };
        public Action<IEvent> OnAfterAdding { get; set; } = @event => { };

        public EventToPublish Read(Guid id)
        {
            return _eventToPublishRepository.Read(id);
        }

        public IList<EventToPublish> GetEventsAwaitingPublishing()
        {
            return _eventToPublishRepository.GetEventsAwaitingPublishing();
        }

        public void Delete(EventToPublish eventToPublish)
        {
            _eventToPublishRepository.Delete(eventToPublish);
        }

        public IUnitOfWork UnitOfWork
        {
            get => _eventToPublishRepository.UnitOfWork;
            set => _eventToPublishRepository.UnitOfWork = value;
        }
    }
}