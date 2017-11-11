using CQRSTutorial.DAL;
using CQRSTutorial.Messaging;
using log4net;

namespace CQRSTutorial.Publish
{
    public class OutboxToMessageQueuePublisher : IOutboxToMessageQueuePublisher
    {
        private readonly IEventToPublishRepository _eventToPublishRepository;
        private readonly MessageBusEventPublisher _messageBusEventPublisher;
        private readonly EventToPublishMapper _eventToPublishMapper;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ILog _logger;

        public OutboxToMessageQueuePublisher(IEventToPublishRepository eventToPublishRepository,
            MessageBusEventPublisher messageBusEventPublisher,
            EventToPublishMapper eventToPublishMapper,
            IUnitOfWorkFactory unitOfWorkFactory,
            ILog logger)
        {
            _eventToPublishRepository = eventToPublishRepository;
            _messageBusEventPublisher = messageBusEventPublisher;
            _eventToPublishMapper = eventToPublishMapper;
            _unitOfWorkFactory = unitOfWorkFactory;
            _logger = logger;
        }

        public void PublishQueuedMessages()
        {
            var eventsToPublishResult = _eventToPublishRepository.GetEventsAwaitingPublishing();
            var eventsToPublish = eventsToPublishResult.EventsToPublish;
            _logger.Debug($"Retrieved {eventsToPublish.Count} events to publish to message queue.");
            foreach (var eventToPublish in eventsToPublish)
            {
                var @event = _eventToPublishMapper.MapToEvent(eventToPublish);
                _logger.Debug($"Publishing event [Id:{@event.Id};Type:{eventToPublish.EventType}]...");
                _messageBusEventPublisher.Handle(new[] { @event });
                using (var unitOfWork = _unitOfWorkFactory.Create().Enrolling(_eventToPublishRepository))
                {
                    unitOfWork.ExecuteInTransaction(() =>
                    {
                        _eventToPublishRepository.Delete(eventToPublish);
                    });
                }
            }
        }
    }
}
