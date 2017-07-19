using System;
using CQRSTutorial.DAL;
using CQRSTutorial.Infrastructure;

namespace Cafe.Publisher
{
    public class OutboxToMessageQueuePublisher
    {
        private readonly EventRepository _repository;
        private readonly MessageBusEventPublisher _messageBusEventPublisher;
        private readonly EventToPublishMapper _eventToPublishMapper;

        public OutboxToMessageQueuePublisher(EventRepository repository, MessageBusEventPublisher messageBusEventPublisher, EventToPublishMapper eventToPublishMapper)
        {
            _repository = repository;
            _messageBusEventPublisher = messageBusEventPublisher;
            _eventToPublishMapper = eventToPublishMapper;
        }

        public void PublishQueuedMessages()
        {
            var eventsToPublish = _repository.GetEventsAwaitingPublishing();
            foreach (var eventToPublish in eventsToPublish)
            {
                var @event = _eventToPublishMapper.MapToEvent(eventToPublish);
                Console.WriteLine($"Publishing event [Id:{@event.Id};Type:{eventToPublish.EventType}]..."); // TODO need to see how to specify queue / channel / topic when publishing
                _messageBusEventPublisher.Receive(new []{ @event });
                _repository.Delete(eventToPublish);
            }
        }
    }
}
