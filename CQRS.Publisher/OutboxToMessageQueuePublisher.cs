using System;
using CQRSTutorial.DAL;
using CQRSTutorial.Infrastructure;
using log4net;

namespace CQRSTutorial.Publisher
{
    public class OutboxToMessageQueuePublisher : IOutboxToMessageQueuePublisher
    {
        private readonly IEventToPublishRepository _eventToPublishRepository;
        private readonly MessageBusEventPublisher _messageBusEventPublisher;
        private readonly EventToPublishMapper _eventToPublishMapper;
        private readonly Func<IUnitOfWork> _createUnitOfWork;
        private readonly int _batchSize;
        private readonly ILog _logger;

        public OutboxToMessageQueuePublisher(IEventToPublishRepository eventToPublishRepository,
            MessageBusEventPublisher messageBusEventPublisher,
            EventToPublishMapper eventToPublishMapper,
            Func<IUnitOfWork> createUnitOfWork,
            IOutboxToMessageQueuePublisherConfiguration outboxToMessageQueuePublisherConfiguration,
            ILog logger)
        {
            _eventToPublishRepository = eventToPublishRepository;
            _messageBusEventPublisher = messageBusEventPublisher;
            _eventToPublishMapper = eventToPublishMapper;
            _createUnitOfWork = createUnitOfWork;
            _logger = logger;
            _batchSize = outboxToMessageQueuePublisherConfiguration.BatchSize;
            _logger.Info($"Batch size: {_batchSize}");
        }

        public void PublishQueuedMessages()
        {
            var firstPass = true;
            EventsToPublishResult eventsToPublishResult = null;
            while (firstPass || eventsToPublishResult.TotalNumberOfEventsToPublish > _batchSize)
            {
                eventsToPublishResult = _eventToPublishRepository.GetEventsAwaitingPublishing(_batchSize);
                var eventsToPublish = eventsToPublishResult.EventsToPublish;
                _logger.Debug($"Retrieved {eventsToPublish.Count} events to publish to message queue.");
                foreach (var eventToPublish in eventsToPublish)
                {
                    var @event = _eventToPublishMapper.MapToEvent(eventToPublish);
                    _logger.Debug($"Publishing event [Id:{@event.Id};Type:{eventToPublish.EventType}]...");
                    _messageBusEventPublisher.Receive(new[] { @event });
                    using (var unitOfWork = _createUnitOfWork())
                    {
                        unitOfWork.Start();
                        _eventToPublishRepository.UnitOfWork = unitOfWork;
                        _eventToPublishRepository.Delete(eventToPublish);
                        unitOfWork.Commit();
                    }
                }
                firstPass = false;
            }
        }
    }
}
