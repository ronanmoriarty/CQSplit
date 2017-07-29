using System;
using CQRSTutorial.DAL;
using CQRSTutorial.Infrastructure;

namespace CQRSTutorial.Publisher
{
    public class OutboxToMessageQueuePublisher
    {
        private readonly IEventToPublishRepository _eventToPublishRepository;
        private readonly MessageBusEventPublisher _messageBusEventPublisher;
        private readonly EventToPublishMapper _eventToPublishMapper;
        private readonly Func<IUnitOfWork> _createUnitOfWork;
        private readonly IOutboxToMessageQueuePublisherConfiguration _outboxToMessageQueuePublisherConfiguration;

        public OutboxToMessageQueuePublisher(IEventToPublishRepository eventToPublishRepository,
            MessageBusEventPublisher messageBusEventPublisher,
            EventToPublishMapper eventToPublishMapper,
            Func<IUnitOfWork> createUnitOfWork,
            IOutboxToMessageQueuePublisherConfiguration outboxToMessageQueuePublisherConfiguration)
        {
            _eventToPublishRepository = eventToPublishRepository;
            _messageBusEventPublisher = messageBusEventPublisher;
            _eventToPublishMapper = eventToPublishMapper;
            _createUnitOfWork = createUnitOfWork;
            _outboxToMessageQueuePublisherConfiguration = outboxToMessageQueuePublisherConfiguration;
        }

        public void PublishQueuedMessages()
        {
            var eventsToPublish = _eventToPublishRepository.GetEventsAwaitingPublishing(_outboxToMessageQueuePublisherConfiguration.BatchSize);
            foreach (var eventToPublish in eventsToPublish)
            {
                var @event = _eventToPublishMapper.MapToEvent(eventToPublish);
                Console.WriteLine($"Publishing event [Id:{@event.Id};Type:{eventToPublish.EventType}]...");
                _messageBusEventPublisher.Receive(new []{ @event });
                using (var unitOfWork = _createUnitOfWork())
                {
                    unitOfWork.Start();
                    _eventToPublishRepository.UnitOfWork = unitOfWork;
                    _eventToPublishRepository.Delete(eventToPublish);
                    unitOfWork.Commit();
                }
            }
        }
    }
}
