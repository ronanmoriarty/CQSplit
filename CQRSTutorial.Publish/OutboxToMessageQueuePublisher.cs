using CQRSTutorial.DAL;
using log4net;
using MassTransit;

namespace CQRSTutorial.Publish
{
    public class OutboxToMessageQueuePublisher : IOutboxToMessageQueuePublisher
    {
        private readonly IEventToPublishRepository _eventToPublishRepository;
        private readonly IBusControl _busControl;
        private readonly EventToPublishSerializer _eventToPublishSerializer;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ILog _logger = LogManager.GetLogger(typeof(OutboxToMessageQueuePublisher));

        public OutboxToMessageQueuePublisher(IEventToPublishRepository eventToPublishRepository,
            IBusControl busControl,
            EventToPublishSerializer eventToPublishSerializer,
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            _eventToPublishRepository = eventToPublishRepository;
            _busControl = busControl;
            _eventToPublishSerializer = eventToPublishSerializer;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public void PublishQueuedMessages()
        {
            var eventsToPublish = _eventToPublishRepository.GetEventsAwaitingPublishing();
            _logger.Debug($"Retrieved {eventsToPublish.Count} events to publish to message queue.");
            foreach (var eventToPublish in eventsToPublish)
            {
                var @event = _eventToPublishSerializer.Deserialize(eventToPublish);
                _logger.Debug($"Publishing event [Id:{@event.Id};Type:{eventToPublish.EventType}]...");
                _busControl.Publish(@event);
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
