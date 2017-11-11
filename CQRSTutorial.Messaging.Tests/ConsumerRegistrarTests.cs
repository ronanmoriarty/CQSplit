using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using NUnit.Framework;

namespace CQRSTutorial.Messaging.Tests
{
    [TestFixture]
    public class ConsumerRegistrarTests
    {
        private const string QueueName = "myQueue";
        private const string ErrorQueueName = "myQueue_error";
        public static readonly ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        private ConsumerFactory _consumerFactory;
        private IBusControl _busControl;
        private ConsumerRegistrar _fakeMessageConsumerRegistrar;
        private ConsumerRegistrar _faultsConsumerRegistrar;

        [SetUp]
        public void SetUp()
        {
            _consumerFactory = new ConsumerFactory();
            _busControl = null;
        }

        private class ConsumerFactory : IConsumerFactory
        {
            public object Create(Type typeToCreate)
            {
                // FakeMessageConsumer and FakeMessageFaultConsumer classes are just default blank constructors - no need for IoC.
                return Activator.CreateInstance(typeToCreate);
            }
        }

        [Test]
        public async Task Registers_all_consumers_listed_in_consumerTypeProvider_with_the_queue_from_the_receiveEndpointConfiguration()
        {
            _fakeMessageConsumerRegistrar = CreateConsumerRegistrarToConsumeFakeMessagesOnQueue(QueueName);
            _faultsConsumerRegistrar = CreateConsumerRegistrarToConsumeFakeMessageFaultsOnQueue(ErrorQueueName);

            CreateBus();
            StartBus();

            await SendMessage();
            WaitUntilBusHasProcessedMessageOrTimedOut();

            Assert.That(FakeMessageFaultConsumer.NumberOfFaults, Is.EqualTo(0));
            Assert.That(FakeMessageConsumer.MessageReceived, Is.True);
        }

        private ConsumerRegistrar CreateConsumerRegistrarToConsumeFakeMessagesOnQueue(string queueName)
        {
            return new ConsumerRegistrar(_consumerFactory,
                new OnlyListsFakeMessageConsumer(),
                new TestReceiveEndpointConfiguration(queueName));
        }

        private class OnlyListsFakeMessageConsumer : IConsumerTypeProvider
        {
            public List<Type> GetConsumerTypes()
            {
                return new List<Type> { typeof(FakeMessageConsumer) };
            }
        }

        private class FakeMessageConsumer : IConsumer<FakeMessage>
        {
            public static bool MessageReceived;
            public async Task Consume(ConsumeContext<FakeMessage> context)
            {
                MessageReceived = true;
                AllowTestThreadToContinueToAssertions();
            }

            private static void AllowTestThreadToContinueToAssertions()
            {
                ManualResetEvent.Set();
            }
        }

        private ConsumerRegistrar CreateConsumerRegistrarToConsumeFakeMessageFaultsOnQueue(string errorQueueName)
        {
            return new ConsumerRegistrar(_consumerFactory,
                new OnlyListsFakeMessageFaultConsumer(),
                new TestReceiveEndpointConfiguration(errorQueueName));
        }

        private class OnlyListsFakeMessageFaultConsumer : IConsumerTypeProvider
        {
            public List<Type> GetConsumerTypes()
            {
                return new List<Type> { typeof(FakeMessageFaultConsumer) };
            }
        }

        private class FakeMessageFaultConsumer : IConsumer<Fault<FakeMessage>>
        {
            public static int NumberOfFaults;

            public async Task Consume(ConsumeContext<Fault<FakeMessage>> context)
            {
                NumberOfFaults++;
                AllowTestThreadToContinueToAssertions();
            }

            private void AllowTestThreadToContinueToAssertions()
            {
                ManualResetEvent.Set();
            }
        }

        private class TestReceiveEndpointConfiguration : IReceiveEndpointConfiguration
        {
            public TestReceiveEndpointConfiguration(string queueName)
            {
                QueueName = queueName;
            }

            public string QueueName { get; }
        }

        private void CreateBus()
        {
            var inMemoryMessageBusFactory = CreateInMemoryMessageBusFactory();
            _busControl = inMemoryMessageBusFactory.Create();
        }

        private InMemoryMessageBusFactory CreateInMemoryMessageBusFactory()
        {
            return new InMemoryMessageBusFactory(
                CreateInMemoryReceiveEndpointsConfigurator(_fakeMessageConsumerRegistrar),
                CreateInMemoryReceiveEndpointsConfigurator(_faultsConsumerRegistrar));
        }

        private InMemoryReceiveEndpointsConfigurator CreateInMemoryReceiveEndpointsConfigurator(ConsumerRegistrar consumerRegistrar)
        {
            return new InMemoryReceiveEndpointsConfigurator(consumerRegistrar);
        }

        private void StartBus()
        {
            _busControl.Start();
        }

        private async Task SendMessage()
        {
            var sendEndpoint = await _busControl.GetSendEndpoint(new Uri($"loopback://localhost/{QueueName}"));
            await sendEndpoint.Send(new FakeMessage());
        }

        private void WaitUntilBusHasProcessedMessageOrTimedOut()
        {
            ManualResetEvent.WaitOne(TimeSpan.FromSeconds(5));
        }

        private class FakeMessage
        {
        }

        [TearDown]
        public void TearDown()
        {
            _busControl?.Stop();
        }
    }
}