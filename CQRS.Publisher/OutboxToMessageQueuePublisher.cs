using System;
using CQRSTutorial.DAL;
using CQRSTutorial.Infrastructure;

namespace CQRSTutorial.Publisher
{
    public class OutboxToMessageQueuePublisher
    {
        private readonly EventToPublishRepository _eventToPublishRepository;
        private readonly MessageBusEventPublisher _messageBusEventPublisher;
        private readonly EventToPublishMapper _eventToPublishMapper;
        private readonly Func<IUnitOfWork> _createUnitOfWork;

        public OutboxToMessageQueuePublisher(EventToPublishRepository eventToPublishRepository,
            MessageBusEventPublisher messageBusEventPublisher,
            EventToPublishMapper eventToPublishMapper,
            Func<IUnitOfWork> createUnitOfWork)
        {
            _eventToPublishRepository = eventToPublishRepository;
            _messageBusEventPublisher = messageBusEventPublisher;
            _eventToPublishMapper = eventToPublishMapper;
            _createUnitOfWork = createUnitOfWork;
        }

        public void PublishQueuedMessages()
        {
            var eventsToPublish = _eventToPublishRepository.GetEventsAwaitingPublishing();
            foreach (var eventToPublish in eventsToPublish)
            {
                var @event = _eventToPublishMapper.MapToEvent(eventToPublish);
                Console.WriteLine($"Publishing event [Id:{@event.Id};Type:{eventToPublish.EventType}]..."); // TODO need to see how to specify queue / channel / topic when publishing
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
