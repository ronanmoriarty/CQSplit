using CQSplit.Messaging;
using CQSplit.Publish;
using MassTransit;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.Command.Service.Tests
{
    [TestFixture]
    public class WaiterCommandServiceTests
    {
        private WaiterCommandService _waiterCommandService;
        private IMessageBusFactory _messageBusFactory;
        private IBusControl _busControl;
        private IOutboxToMessageBusPublisher _outboxToMessageBusPublisher;

        [SetUp]
        public void SetUp()
        {
            _busControl = Substitute.For<IBusControl>();
            _messageBusFactory = Substitute.For<IMessageBusFactory>();
            _messageBusFactory.Create().Returns(_busControl);
            _outboxToMessageBusPublisher = Substitute.For<IOutboxToMessageBusPublisher>();
            _waiterCommandService = new WaiterCommandService(_busControl, _outboxToMessageBusPublisher);
        }

        [Test]
        public void Starting_waiter_service_publishes_any_existing_messages_in_outbox()
        {
            _waiterCommandService.Start();

            _outboxToMessageBusPublisher.Received(1).PublishQueuedMessages();
        }

        [Test]
        public void Stopping_waiter_service_stops_the_bus()
        {
            _waiterCommandService.Start();
            _waiterCommandService.Stop();

            _busControl.Received(1).Stop();
        }
    }
}
