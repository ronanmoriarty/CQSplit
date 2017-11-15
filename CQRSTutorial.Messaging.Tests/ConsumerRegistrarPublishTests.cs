using System;
using System.Collections.Generic;
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
        private IConsumerFactory _consumerFactory;
        private IBusControl _busControl;
        private ConsumerRegistrar _consumerRegistrar;
        private ConsumerRegistrar _faultConsumerRegistrar;

        [SetUp]
        public void SetUp()
        {
            _consumerFactory = new DefaultConstructorConsumerFactory();
        }

        [Test]
        public async Task Registers_all_consumers_listed_in_consumerTypeProvider_to_receive_published_messages()
        {
            _consumerRegistrar = CreateConsumerRegistrarToConsumeFakeEventsOnQueue(QueueName);
            _faultConsumerRegistrar = CreateConsumerRegistrarToConsumeFakeEventFaultsOnQueue(ErrorQueueName);

            CreateBus();
            StartBus();

            await PublishMessage();
            WaitUntilBusHasProcessedMessageOrTimedOut();

            Assert.That(FakeEventFaultConsumer.NumberOfFaults, Is.EqualTo(0));
            Assert.That(FakeEventConsumer.EventReceived, Is.True);
        }

        private ConsumerRegistrar CreateConsumerRegistrarToConsumeFakeEventsOnQueue(string queueName)
        {
            return new ConsumerRegistrar(_consumerFactory,
                new OnlyListsFakeEventConsumer(),
                new TestReceiveEndpointConfiguration(queueName));
        }

        private class OnlyListsFakeEventConsumer : IConsumerTypeProvider
        {
            public List<Type> GetConsumerTypes()
            {
                return new List<Type>
                {
                    typeof(FakeEventConsumer)
                };
            }
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

        private ConsumerRegistrar CreateConsumerRegistrarToConsumeFakeEventFaultsOnQueue(string errorQueueName)
        {
            return new ConsumerRegistrar(_consumerFactory,
                new OnlyListsFakeEventFaultConsumer(),
                new TestReceiveEndpointConfiguration(errorQueueName));
        }

        private class OnlyListsFakeEventFaultConsumer : IConsumerTypeProvider
        {
            public List<Type> GetConsumerTypes()
            {
                return new List<Type>
                {
                    typeof(FakeEventFaultConsumer)
                };
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

        private class TestReceiveEndpointConfiguration : IReceiveEndpointConfiguration
        {
            public TestReceiveEndpointConfiguration(string queueName)
            {
                QueueName = queueName;
            }

            public string QueueName { get; }
        }

        private InMemoryMessageBusFactory CreateInMemoryMessageBusFactory()
        {
            return new InMemoryMessageBusFactory(
                CreateInMemoryReceiveEndpointsConfigurator(_consumerRegistrar),
                CreateInMemoryReceiveEndpointsConfigurator(_faultConsumerRegistrar));
        }

        private InMemoryReceiveEndpointsConfigurator CreateInMemoryReceiveEndpointsConfigurator(ConsumerRegistrar consumerRegistrar)
        {
            return new InMemoryReceiveEndpointsConfigurator(consumerRegistrar);
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