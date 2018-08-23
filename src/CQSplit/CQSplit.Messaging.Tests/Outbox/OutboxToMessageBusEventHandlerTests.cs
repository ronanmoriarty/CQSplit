using CQSplit.Messaging.Outbox;
using NSubstitute;
using NUnit.Framework;

namespace CQSplit.Messaging.Tests.Outbox
{
    [TestFixture]
    public class OutboxToMessageBusEventHandlerTests
    {
        private OutboxToMessageBusEventHandler _outboxToMessageBusEventHandler;
        private IOutboxToMessageBusPublisher _outboxToMessageBusPublisher;

        [Test]
        public void Wraps_underlying_publisher()
        {
            _outboxToMessageBusPublisher = Substitute.For<IOutboxToMessageBusPublisher>();
            _outboxToMessageBusEventHandler = new OutboxToMessageBusEventHandler(_outboxToMessageBusPublisher);
            var events = new IEvent[]{};

            _outboxToMessageBusEventHandler.Handle(events);

            _outboxToMessageBusPublisher.Received(1).PublishEvents(events);
        }
    }
}