using System;
using System.Threading;
using System.Threading.Tasks;
using CQRSTutorial.Messaging.Tests.Common;
using MassTransit;
using NUnit.Framework;

namespace CQRSTutorial.Messaging.Tests
{
    [TestFixture]
    public class ConsumerRegistrarPublishTests
    {
        private const string QueueName = "myQueue";
        private const string ErrorQueueName = "myQueue_error";
        public static readonly ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        private IBusControl _busControl;
        private ConsumerRegistrar _consumerRegistrar;
        private ConsumerRegistrar _faultEventConsumerRegistrar;
        private Common.Consumer<FakeEvent> _fakeEventConsumer;
        private Common.Consumer<Fault<FakeEvent>> _fakeEventFaultConsumer;

        [SetUp]
        public void SetUp()
        {
            _fakeEventConsumer = new Common.Consumer<FakeEvent>(ManualResetEvent);
            _consumerRegistrar = ConsumerRegistrarFactory.Create(QueueName, _fakeEventConsumer);
            _fakeEventFaultConsumer = new Common.Consumer<Fault<FakeEvent>>(ManualResetEvent);
            _faultEventConsumerRegistrar = ConsumerRegistrarFactory.Create(ErrorQueueName, _fakeEventFaultConsumer);

            CreateBus();
            _busControl.Start();
        }

        private void CreateBus()
        {
            var inMemoryMessageBusFactory = CreateInMemoryMessageBusFactory();
            _busControl = inMemoryMessageBusFactory.Create();
        }

        private InMemoryMessageBusFactory CreateInMemoryMessageBusFactory()
        {
            return new InMemoryMessageBusFactory(
                new InMemoryReceiveEndpointsConfigurator(_consumerRegistrar),
                new InMemoryReceiveEndpointsConfigurator(_faultEventConsumerRegistrar));
        }

        [Test]
        public async Task Registers_all_consumers_listed_in_consumerTypeProvider_to_receive_published_messages()
        {
            await PublishMessage();
            WaitUntilBusHasProcessedMessageOrTimedOut();

            Assert.That(_fakeEventConsumer.ReceivedMessage, Is.True);
            Assert.That(_fakeEventFaultConsumer.ReceivedMessage, Is.False);
        }

        private async Task PublishMessage()
        {
            await _busControl.Publish(new FakeEvent());
        }

        private void WaitUntilBusHasProcessedMessageOrTimedOut()
        {
            ManualResetEvent.WaitOne(TimeSpan.FromSeconds(5));
        }

        private class FakeEvent
        {
        }

        [TearDown]
        public void TearDown()
        {
            _busControl?.Stop();
        }
    }
}