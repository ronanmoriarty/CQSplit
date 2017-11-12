using System.Collections.Generic;
using CQRSTutorial.DAL;
using MassTransit;
using NSubstitute;
using NUnit.Framework;

namespace CQRSTutorial.Publish.Tests
{
    [TestFixture]
    public class OutboxToMessagePublisherTests
    {
        private IEventToPublishRepository _eventToPublishRepository;
        private TestEvent _event;
        private IEventToPublishSerializer _eventToPublishSerializer;
        private IBusControl _busControl;
        private OutboxToMessageQueuePublisher _outboxToMessagePublisher;
        private EventToPublish _eventToPublish;

        [SetUp]
        public void SetUp()
        {
            _eventToPublishSerializer = Substitute.For<IEventToPublishSerializer>();
            _event = new TestEvent();
            _eventToPublishRepository = Substitute.For<IEventToPublishRepository>();
            _eventToPublish = new EventToPublish();
            _eventToPublishRepository
                .GetEventsAwaitingPublishing()
                .Returns(new List<EventToPublish> { _eventToPublish });
            _eventToPublishSerializer.Deserialize(_eventToPublish).Returns(_event);
            _busControl = Substitute.For<IBusControl>();
            _outboxToMessagePublisher = new OutboxToMessageQueuePublisher(
                _eventToPublishRepository,
                _busControl,
                _eventToPublishSerializer,
                null);
        }

        [Test]
        public void Publisher_publishes_event()
        {
            _outboxToMessagePublisher.PublishQueuedMessages();

            _busControl.Received(1).Publish(_event);
        }
    }
}
