using System;
using System.Threading;
using System.Threading.Tasks;
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
        private ConsumerRegistrar _faultConsumerRegistrar;

        [SetUp]
        public void SetUp()
        {
            _consumerRegistrar = ConsumerRegistrarFactory.Create(QueueName, typeof(FakeEventConsumer));
            _faultConsumerRegistrar = ConsumerRegistrarFactory.Create(ErrorQueueName, typeof(FakeEventFaultConsumer));

            CreateBus();
            StartBus();
        }

        private class FakeEventConsumer : IConsumer<FakeEvent>
        {
            public static bool EventReceived;
            public async Task Consume(ConsumeContext<FakeEvent> context)
            {
                EventReceived = true;
                AllowTestThreadToContinueToAssertions();
            }

            private static void AllowTestThreadToContinueToAssertions()
            {
                ManualResetEvent.Set();
            }
        }

        private class FakeEventFaultConsumer : IConsumer<Fault<FakeEvent>>
        {
            public static int NumberOfFaults;

            public async Task Consume(ConsumeContext<Fault<FakeEvent>> context)
            {
                NumberOfFaults++;
                AllowTestThreadToContinueToAssertions();
            }

            private void AllowTestThreadToContinueToAssertions()
            {
                ManualResetEvent.Set();
            }
        }

        private class FakeEvent
        {
        }

        private void StartBus()
        {
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
                new InMemoryReceiveEndpointsConfigurator(_faultConsumerRegistrar));
        }

        [Test]
        public async Task Registers_all_consumers_listed_in_consumerTypeProvider_to_receive_published_messages()
        {
            await PublishMessage();
            WaitUntilBusHasProcessedMessageOrTimedOut();

            Assert.That(FakeEventFaultConsumer.NumberOfFaults, Is.EqualTo(0));
            Assert.That(FakeEventConsumer.EventReceived, Is.True);
        }

        private async Task PublishMessage()
        {
            await _busControl.Publish(new FakeEvent());
        }

        private void WaitUntilBusHasProcessedMessageOrTimedOut()
        {
            ManualResetEvent.WaitOne(TimeSpan.FromSeconds(5));
        }

        [TearDown]
        public void TearDown()
        {
            _busControl?.Stop();
        }
    }
}