using CQRSTutorial.DAL;
using CQRSTutorial.Infrastructure;

namespace Cafe.Publisher
{
    public class OutboxToMessageQueuePublisher
    {
        private readonly EventRepository _repository;
        private readonly MessageBusEventPublisher _messageBusEventPublisher;
        private readonly EventDescriptorMapper _eventDescriptorMapper;

        public OutboxToMessageQueuePublisher(EventRepository repository, MessageBusEventPublisher messageBusEventPublisher, EventDescriptorMapper eventDescriptorMapper)
        {
            _repository = repository;
            _messageBusEventPublisher = messageBusEventPublisher;
            _eventDescriptorMapper = eventDescriptorMapper;
        }

        public void PublishQueuedMessages()
        {
            var eventDescriptors = _repository.ReadEventsAwaitingPublishing();
            foreach (var eventDescriptor in eventDescriptors)
            {
                var @event = _eventDescriptorMapper.MapEventDescriptorToEvent(eventDescriptor);
                _messageBusEventPublisher.Publish(new []{ @event });
            }
        }
    }
}
