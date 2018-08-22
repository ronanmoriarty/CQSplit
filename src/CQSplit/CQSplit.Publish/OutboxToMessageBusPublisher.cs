using System;
using System.Collections.Generic;
using System.Linq;
using CQSplit.DAL;
using MassTransit;
using NLog;

namespace CQSplit.Publish
{
    public class OutboxToMessageBusPublisher : IOutboxToMessageBusPublisher
    {
        private readonly IEventToPublishRepository _eventToPublishRepository;
        private readonly IBusControl _busControl;
        private readonly IEventToPublishSerializer _eventToPublishSerializer;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public OutboxToMessageBusPublisher(IEventToPublishRepository eventToPublishRepository,
            IBusControl busControl,
            IEventToPublishSerializer eventToPublishSerializer)
        {
            _eventToPublishRepository = eventToPublishRepository;
            _busControl = busControl;
            _eventToPublishSerializer = eventToPublishSerializer;
        }

        public void PublishQueuedMessages()
        {
            var events = _eventToPublishRepository
                .GetEventsAwaitingPublishing()
                .Select(eventToPublish => _eventToPublishSerializer.Deserialize(eventToPublish))
                .ToList();
            _logger.Debug($"Retrieved {events.Count} events to publish to message queue.");
            PublishEvents(events);
        }

        public void PublishEvents(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                _logger.Debug($"Publishing event [Id:{@event.Id}; Type:{@event.GetType()}] to {_busControl.Address}...");
                _busControl.Publish((object) @event);
                RemoveEventFromEventToPublishQueue(@event.Id);
            }
        }

        private void RemoveEventFromEventToPublishQueue(Guid eventId)
        {
            _eventToPublishRepository.UnitOfWork.ExecuteInTransaction(() =>
            {
                var retrievedEventToPublish = _eventToPublishRepository.Read(eventId);
                _eventToPublishRepository.Delete(retrievedEventToPublish);
                _logger.Debug($"Removed event [Id: {eventId}] from dbo.EventsToPublish because it has been published to message bus.");
            });
        }
    }
}
