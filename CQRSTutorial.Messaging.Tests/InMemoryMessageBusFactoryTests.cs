using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using NUnit.Framework;

namespace CQRSTutorial.Messaging.Tests
{
    [TestFixture]
    public class InMemoryMessageBusFactoryTests
    {
        private const string QueueName = "whatever";
        private const string ErrorQueueName = "whatever_error";
        public static readonly ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        private ConsumerFactory _consumerFactory;
        private IBusControl _busControl;

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
                return Activator.CreateInstance(typeToCreate); // FakeMessageConsumer and FakeMessageFaultConsumer classes are just default blank constructors - no need for IoC.
            }
        }

        [Test]
        public async Task Registers_all_consumers_listed_in_consumerTypeProvider()
        {
            CreateInMemoryBus();
            StartBus();

            await SendMessage();
            WaitUntilBusHasProcessedMessageOrTimedOut();

            Assert.That(FakeMessageFaultConsumer.NumberOfFaults, Is.EqualTo(0));
            Assert.That(FakeMessageConsumer.MessageReceived, Is.True);
        }

        private void CreateInMemoryBus()
        {
            var inMemoryMessageBusFactory = CreateInMemoryMessageBusFactory();
            _busControl = inMemoryMessageBusFactory.Create();
        }

        private InMemoryMessageBusFactory CreateInMemoryMessageBusFactory()
        {
            return new InMemoryMessageBusFactory(
                ConfigureBusToReceiveFakeMessages(),
                ConfigureBusToReceiveFaults());
        }

        private InMemoryReceiveEndpointsConfigurator ConfigureBusToReceiveFakeMessages()
        {
            return new InMemoryReceiveEndpointsConfigurator(
                new ConsumerRegistrar(_consumerFactory, new OnlyListsFakeMessageConsumer(), new TestReceiveEndpointConfiguration(QueueName)),
                QueueName);
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

        private InMemoryReceiveEndpointsConfigurator ConfigureBusToReceiveFaults()
        {
            return new InMemoryReceiveEndpointsConfigurator(
                new ConsumerRegistrar(_consumerFactory, new OnlyListsFakeMessageFaultConsumer(), new TestReceiveEndpointConfiguration(ErrorQueueName)),
                ErrorQueueName);
        }

        private class TestReceiveEndpointConfiguration : IReceiveEndpointConfiguration
        {
            public TestReceiveEndpointConfiguration(string queueName)
            {
                QueueName = queueName;
            }

            public string QueueName { get; }
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

        private void StartBus()
        {
            _busControl.Start();
        }

        private async Task SendMessage()
        {
            var sendEndpoint = await _busControl.GetSendEndpoint(new Uri($"loopback://localhost/{QueueName}"));
            await sendEndpoint.Send(new FakeMessage());
        }

        private class FakeMessage
        {
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