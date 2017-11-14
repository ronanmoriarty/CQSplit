using System.Collections.Generic;
using CQRSTutorial.Core;
using CQRSTutorial.DAL;
using log4net;
using MassTransit;

namespace CQRSTutorial.Publish
{
    public class OutboxToMessageBusPublisher : IOutboxToMessageBusPublisher
    {
        private readonly IEventToPublishRepository _eventToPublishRepository;
        private readonly IBusControl _busControl;
        private readonly IEventToPublishSerializer _eventToPublishSerializer;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly ILog _logger = LogManager.GetLogger(typeof(OutboxToMessageBusPublisher));

        public OutboxToMessageBusPublisher(IEventToPublishRepository eventToPublishRepository,
            IBusControl busControl,
            IEventToPublishSerializer eventToPublishSerializer,
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
                RemoveEventFromEventToPublishQueue(eventToPublish);
            }
        }

        public void PublishEvents(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                _busControl.Publish((object) @event);
                var eventToPublish = _eventToPublishSerializer.Serialize(@event);
                RemoveEventFromEventToPublishQueue(eventToPublish);
            }
        }

        private void RemoveEventFromEventToPublishQueue(EventToPublish eventToPublish)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create().Enrolling(_eventToPublishRepository))
            {
                unitOfWork.ExecuteInTransaction(() =>
                {
                    var retrievedEventToPublish = _eventToPublishRepository.Read(eventToPublish.Id);
                    _eventToPublishRepository.Delete(retrievedEventToPublish);
                });
            }
        }
    }
}
