using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using NUnit.Framework;

namespace CQRSTutorial.Messaging.Tests
{
    [TestFixture]
    public class ConsumerRegistrarSendTests
    {
        private const string QueueName = "myQueue";
        private const string ErrorQueueName = "myQueue_error";
        public static readonly ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        private ConsumerFactory _consumerFactory;
        private IBusControl _busControl;
        private ConsumerRegistrar _consumerRegistrar;
        private ConsumerRegistrar _faultConsumerRegistrar;

        [SetUp]
        public void SetUp()
        {
            _consumerFactory = new ConsumerFactory();
        }

        private class ConsumerFactory : IConsumerFactory
        {
            public object Create(Type typeToCreate)
            {
                // FakeCommandConsumer and FakeCommandFaultConsumer classes are just default blank constructors - no need for IoC.
                return Activator.CreateInstance(typeToCreate);
            }
        }

        [Test]
        public async Task Registers_all_consumers_listed_in_consumerTypeProvider_with_the_queue_from_the_receiveEndpointConfiguration()
        {
            _consumerRegistrar = CreateConsumerRegistrarToConsumeFakeCommandsOnQueue(QueueName);
            _faultConsumerRegistrar = CreateConsumerRegistrarToConsumeFakeCommandFaultsOnQueue(ErrorQueueName);

            CreateBus();
            StartBus();

            await SendMessage();
            WaitUntilBusHasProcessedMessageOrTimedOut();

            Assert.That(FakeCommandFaultConsumer.NumberOfFaults, Is.EqualTo(0));
            Assert.That(FakeCommandConsumer.CommandReceived, Is.True);
        }

        private ConsumerRegistrar CreateConsumerRegistrarToConsumeFakeCommandsOnQueue(string queueName)
        {
            return new ConsumerRegistrar(_consumerFactory,
                new OnlyListsFakeCommandConsumer(),
                new TestReceiveEndpointConfiguration(queueName));
        }

        private class OnlyListsFakeCommandConsumer : IConsumerTypeProvider
        {
            public List<Type> GetConsumerTypes()
            {
                return new List<Type>
                {
                    typeof(FakeCommandConsumer)
                };
            }
        }

        private class FakeCommandConsumer : IConsumer<FakeCommand>
        {
            public static bool CommandReceived;
            public async Task Consume(ConsumeContext<FakeCommand> context)
            {
                CommandReceived = true;
                AllowTestThreadToContinueToAssertions();
            }

            private static void AllowTestThreadToContinueToAssertions()
            {
                ManualResetEvent.Set();
            }
        }

        private ConsumerRegistrar CreateConsumerRegistrarToConsumeFakeCommandFaultsOnQueue(string errorQueueName)
        {
            return new ConsumerRegistrar(_consumerFactory,
                new OnlyListsFakeCommandFaultConsumer(),
                new TestReceiveEndpointConfiguration(errorQueueName));
        }

        private class OnlyListsFakeCommandFaultConsumer : IConsumerTypeProvider
        {
            public List<Type> GetConsumerTypes()
            {
                return new List<Type>
                {
                    typeof(FakeCommandFaultConsumer)
                };
            }
        }

        private class FakeCommandFaultConsumer : IConsumer<Fault<FakeCommand>>
        {
            public static int NumberOfFaults;

            public async Task Consume(ConsumeContext<Fault<FakeCommand>> context)
            {
                NumberOfFaults++;
                AllowTestThreadToContinueToAssertions();
            }

            private void AllowTestThreadToContinueToAssertions()
            {
                ManualResetEvent.Set();
            }
        }

        private class FakeCommand
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

        private async Task SendMessage()
        {
            var sendEndpoint = await _busControl.GetSendEndpoint(new Uri($"loopback://localhost/{QueueName}"));
            await sendEndpoint.Send(new FakeCommand());
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