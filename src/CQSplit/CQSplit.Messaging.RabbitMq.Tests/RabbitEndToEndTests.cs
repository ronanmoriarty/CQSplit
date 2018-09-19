using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CQSplit.Messaging.Tests.Common;
using MassTransit;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace CQSplit.Messaging.RabbitMq.Tests
{
    [TestFixture]
    [Category("Integration")]
    public class RabbitEndToEndTests
    {
        private RabbitMqSendEndpointProvider _rabbitMqSendEndpointProvider;
        private IBusControl _busControl;
        private Consumer<TestMessage> _consumer;
        private Semaphore _semaphore;
        private readonly Guid _id = Guid.NewGuid();
        private const string QueueName = "RabbitMqMessageBusFactoryTests_Queue";

        [SetUp]
        public void SetUp()
        {
            _semaphore = new Semaphore(0, 1);
            _consumer = new Consumer<TestMessage>(_semaphore);
            IConsumer[] consumers = { _consumer };

            var rabbitMqHostConfiguration = CreateRabbitMqHostConfiguration();

            var rabbitMqMessageBusFactory = new RabbitMqMessageBusFactory(
                rabbitMqHostConfiguration,
                new RabbitMqReceiveEndpointConfigurator(
                    new ConsumerRegistrar(
                        new PreviouslyConstructedConsumerFactory(consumers),
                        new ConsumerTypeProvider(typeof(Consumer<TestMessage>)),
                        new ReceiveEndpointConfiguration(QueueName)
                    )
                )
            );

            _busControl = new MultipleConnectionAttemptMessageBusFactory(rabbitMqMessageBusFactory).Create();
            _rabbitMqSendEndpointProvider = new RabbitMqSendEndpointProvider(_busControl, rabbitMqHostConfiguration);
        }

        [Test]
        public async Task Sent_messages_are_received()
        {
            var sendEndpoint = await _rabbitMqSendEndpointProvider.GetSendEndpoint(QueueName);

            await sendEndpoint.Send(new TestMessage { Id = _id });
            WaitUntilMessageHasBeenConsumed();

            Assert.That(_consumer.ReceivedMessage);
            Assert.That(_consumer.ReceivedMessages.Single(message => message.Id == _id), Is.Not.Null);
        }

        public class TestMessage
        {
            public Guid Id { get; set; }
        }

        private static RabbitMqHostConfiguration CreateRabbitMqHostConfiguration()
        {
            var configurationRoot = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            return new RabbitMqHostConfiguration(configurationRoot);
        }

        private void WaitUntilMessageHasBeenConsumed()
        {
            var timedOut = !_semaphore.WaitOne(TimeSpan.FromSeconds(5));
            if (timedOut)
            {
                Assert.Fail("Timed out");
            }
        }
    }
}
