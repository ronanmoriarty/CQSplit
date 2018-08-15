using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CQSplit.Messaging.Tests.Common;
using MassTransit;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace CQSplit.Messaging.RabbitMq.IntegrationTests
{
    [TestFixture]
    [Category("Integration")]
    public class RabbitEndToEndTests
    {
        private RabbitMqSendEndpointProvider _rabbitMqSendEndpointProvider;
        private IBusControl _busControl;
        private SemaphoreConsumer<TestMessage> _consumer;
        private Semaphore _semaphore;
        private const string QueueName = "RabbitMqMessageBusFactoryTests_Queue";

        [SetUp]
        public void SetUp()
        {
            _semaphore = new Semaphore(0, 1);
            _consumer = new SemaphoreConsumer<TestMessage>(_semaphore);
            IConsumer[] consumers = { _consumer };

            var rabbitMqHostConfiguration = CreateRabbitMqHostConfiguration();

            var rabbitMqMessageBusFactory = new RabbitMqMessageBusFactory(
                rabbitMqHostConfiguration,
                new RabbitMqReceiveEndpointConfigurator(
                    new ConsumerRegistrar(
                        new PreviouslyConstructedConsumerFactory(consumers),
                        new ConsumerTypeProvider(typeof(SemaphoreConsumer<TestMessage>)),
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

            var id = Guid.NewGuid();
            await sendEndpoint.Send(new TestMessage { Id = id });

            var timedOut = !_semaphore.WaitOne(TimeSpan.FromSeconds(5));
            if (timedOut)
            {
                Assert.Fail("Timed out");
            }

            Assert.That(_consumer.ReceivedMessage);
            Assert.That(_consumer.ReceivedMessages.Single(message => message.Id == id), Is.Not.Null);
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

            var rabbitMqHostConfiguration = new RabbitMqHostConfiguration(configurationRoot);
            return rabbitMqHostConfiguration;
        }
    }
}
