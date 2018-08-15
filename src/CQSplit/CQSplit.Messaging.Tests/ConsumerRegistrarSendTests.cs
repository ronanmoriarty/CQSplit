using System;
using System.Threading;
using System.Threading.Tasks;
using CQSplit.Messaging.Tests.Common;
using MassTransit;
using NUnit.Framework;

namespace CQSplit.Messaging.Tests
{
    [TestFixture]
    public class ConsumerRegistrarSendTests
    {
        private const string QueueName = "myQueue";
        private const string ErrorQueueName = "myQueue_error";
        private const string LoopbackAddress = "loopback://localhost/";
        public static readonly Semaphore Semaphore = new Semaphore(0, 1);
        public static readonly Semaphore Semaphore2 = new Semaphore(0, 1);
        private IBusControl _busControl;
        private ConsumerRegistrar _consumerRegistrar;
        private ConsumerRegistrar _faultConsumerRegistrar;
        private Consumer<FakeCommand> _fakeCommandConsumer;
        private Consumer<FakeCommand2> _fakeCommand2Consumer;
        private Consumer<Fault<FakeCommand>> _fakeCommandFaultConsumer;
        private Consumer<Fault<FakeCommand2>> _fakeCommand2FaultConsumer;

        [SetUp]
        public void SetUp()
        {
            _fakeCommandConsumer = new Consumer<FakeCommand>(Semaphore);
            _fakeCommand2Consumer = new Consumer<FakeCommand2>(Semaphore2);
            _consumerRegistrar = ConsumerRegistrarFactory.Create(QueueName, _fakeCommandConsumer, _fakeCommand2Consumer);
            _fakeCommandFaultConsumer = new Consumer<Fault<FakeCommand>>(Semaphore);
            _fakeCommand2FaultConsumer = new Consumer<Fault<FakeCommand2>>(Semaphore2);
            _faultConsumerRegistrar = ConsumerRegistrarFactory.Create(ErrorQueueName, _fakeCommandFaultConsumer, _fakeCommand2FaultConsumer);

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
                new InMemoryReceiveEndpointsConfigurator(_faultConsumerRegistrar));
        }

        [Test]
        public async Task Registers_all_consumers_listed_in_consumerTypeProvider_with_the_queue_from_the_receiveEndpointConfiguration()
        {
            await SendFakeCommand();
            WaitUntilBusHasProcessedMessageOrTimedOut(Semaphore);

            Assert.That(_fakeCommandConsumer.ReceivedMessage, Is.True);
            Assert.That(_fakeCommandFaultConsumer.ReceivedMessage, Is.False);
        }

        private async Task SendFakeCommand()
        {
            var sendEndpoint = await GetSendEndpoint();
            await sendEndpoint.Send(new FakeCommand());
        }

        [Test]
        public async Task Can_register_two_consumers_against_the_same_receive_endpoint()
        {
            await SendFakeCommand2();
            WaitUntilBusHasProcessedMessageOrTimedOut(Semaphore2);

            Assert.That(_fakeCommand2Consumer.ReceivedMessage, Is.True);
            Assert.That(_fakeCommand2FaultConsumer.ReceivedMessage, Is.False);
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

        private void WaitUntilBusHasProcessedMessageOrTimedOut(Semaphore manualResetEvent)
        {
            manualResetEvent.WaitOne(TimeSpan.FromSeconds(5));
        }

        private class FakeCommand
        {
        }

        private class FakeCommand2
        {
        }

        [TearDown]
        public void TearDown()
        {
            _busControl?.Stop();
        }
    }
}