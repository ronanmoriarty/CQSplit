using System;
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
        private const string LoopbackAddress = "loopback://localhost/";
        public static readonly ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        public static readonly ManualResetEvent ManualResetEvent2 = new ManualResetEvent(false);
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
        public async Task Registers_all_consumers_listed_in_consumerTypeProvider_with_the_queue_from_the_receiveEndpointConfiguration()
        {
            _consumerRegistrar = CreateConsumerRegistrarToConsumeFakeCommandsOnQueue(QueueName);
            _faultConsumerRegistrar = CreateConsumerRegistrarToConsumeFakeCommandFaultsOnQueue(ErrorQueueName);

            CreateBus();
            StartBus();

            await SendFakeCommand();
            WaitUntilBusHasProcessedMessageOrTimedOut(ManualResetEvent);

            Assert.That(FakeCommandFaultConsumer.NumberOfFaults, Is.EqualTo(0));
            Assert.That(FakeCommandConsumer.CommandReceived, Is.True);
        }

        [Test]
        public async Task Can_register_two_consumers_against_the_same_receive_endpoint()
        {
            _consumerRegistrar = CreateConsumerRegistrarToConsumeFakeCommandsOnQueue(QueueName);
            _faultConsumerRegistrar = CreateConsumerRegistrarToConsumeFakeCommandFaultsOnQueue(ErrorQueueName);

            CreateBus();
            StartBus();

            await SendFakeCommand2();
            WaitUntilBusHasProcessedMessageOrTimedOut(ManualResetEvent2);

            Assert.That(FakeCommand2FaultConsumer.NumberOfFaults, Is.EqualTo(0));
            Assert.That(FakeCommand2Consumer.CommandReceived, Is.True);
        }

        private ConsumerRegistrar CreateConsumerRegistrarToConsumeFakeCommandsOnQueue(string queueName)
        {
            return new ConsumerRegistrar(_consumerFactory,
                new ConsumerTypeProvider(
                    typeof(FakeCommandConsumer),
                    typeof(FakeCommand2Consumer)
                ),
                new ReceiveEndpointConfiguration(queueName));
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

        private class FakeCommand2Consumer : IConsumer<FakeCommand2>
        {
            public static bool CommandReceived;
            public async Task Consume(ConsumeContext<FakeCommand2> context)
            {
                CommandReceived = true;
                AllowTestThreadToContinueToAssertions();
            }

            private static void AllowTestThreadToContinueToAssertions()
            {
                ManualResetEvent2.Set();
            }
        }

        private ConsumerRegistrar CreateConsumerRegistrarToConsumeFakeCommandFaultsOnQueue(string errorQueueName)
        {
            return new ConsumerRegistrar(_consumerFactory,
                new ConsumerTypeProvider(
                    typeof(FakeCommandFaultConsumer),
                    typeof(FakeCommand2FaultConsumer)
                ),
                new ReceiveEndpointConfiguration(errorQueueName));
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

        private class FakeCommand2FaultConsumer : IConsumer<Fault<FakeCommand2>>
        {
            public static int NumberOfFaults;

            public async Task Consume(ConsumeContext<Fault<FakeCommand2>> context)
            {
                NumberOfFaults++;
                AllowTestThreadToContinueToAssertions();
            }

            private void AllowTestThreadToContinueToAssertions()
            {
                ManualResetEvent2.Set();
            }
        }

        private class FakeCommand
        {
        }

        private class FakeCommand2
        {
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

        private async Task SendFakeCommand()
        {
            var sendEndpoint = await GetSendEndpoint();
            await sendEndpoint.Send(new FakeCommand());
        }

        private async Task SendFakeCommand2()
        {
            var sendEndpoint = await GetSendEndpoint();
            await sendEndpoint.Send(new FakeCommand2());
        }

        private async Task<ISendEndpoint> GetSendEndpoint()
        {
            return await _busControl.GetSendEndpoint(new Uri($"{LoopbackAddress}{QueueName}"));
        }

        private void WaitUntilBusHasProcessedMessageOrTimedOut(ManualResetEvent manualResetEvent)
        {
            manualResetEvent.WaitOne(TimeSpan.FromSeconds(5));
        }

        [TearDown]
        public void TearDown()
        {
            _busControl?.Stop();
        }
    }
}