using System;
using CQRSTutorial.DAL;
using CQRSTutorial.Infrastructure;

namespace Cafe.Publisher
{
    public class OutboxToMessageQueuePublisher
    {
        private readonly EventDescriptorRepository _repository;
        private readonly MessageBusEventPublisher _messageBusEventPublisher;
        private readonly EventDescriptorMapper _eventDescriptorMapper;

        public OutboxToMessageQueuePublisher(EventDescriptorRepository repository, MessageBusEventPublisher messageBusEventPublisher, EventDescriptorMapper eventDescriptorMapper)
        {
            _repository = repository;
            _messageBusEventPublisher = messageBusEventPublisher;
            _eventDescriptorMapper = eventDescriptorMapper;
        }

        public void PublishQueuedMessages()
        {
            var eventDescriptors = _repository.GetEventsAwaitingPublishing();
            foreach (var eventDescriptor in eventDescriptors)
            {
                var @event = _eventDescriptorMapper.MapEventDescriptorToEvent(eventDescriptor);
                Console.WriteLine($"Publishing event [Id:{@event.Id};Type:{eventDescriptor.EventType}] to \"{eventDescriptor.PublishTo}\"...");
                _messageBusEventPublisher.Publish(new []{ @event });
            }
        }
    }
}
