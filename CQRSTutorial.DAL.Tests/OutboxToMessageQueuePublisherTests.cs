using System;
using System.Data;
using System.Threading;
using Cafe.Domain.Events;
using Cafe.Publisher;
using CQRSTutorial.Core;
using CQRSTutorial.Infrastructure;
using MassTransit;
using MassTransit.RabbitMqTransport;
using NHibernate;
using NUnit.Framework;

namespace CQRSTutorial.DAL.Tests
{
    [TestFixture]
    public class OutboxToMessageQueuePublisherTests : InsertAndReadTest<EventRepository, EventDescriptor>
    {
        private OutboxToMessageQueuePublisher _outboxToMessageQueuePublisher;
        private readonly IPublishConfiguration _publishConfiguration;
        private readonly string _publishLocation = $"{nameof(OutboxToMessageQueuePublisherTests)}_queue";
        private int _messagesPublished;

        public OutboxToMessageQueuePublisherTests()
        {
            _publishConfiguration = new TestPublishConfiguration(_publishLocation);
        }

        protected override EventRepository CreateRepository(ISessionFactory readSessionFactory, IsolationLevel isolationLevel)
        {
            return new EventRepository(readSessionFactory, isolationLevel, _publishConfiguration, new EventDescriptorMapper());
        }

        protected override void AdditionalSetup()
        {
            var messageBusEventPublisher = new MessageBusEventPublisher(new MessageBusFactory(new EnvironmentVariableMessageBusConfiguration(), ConfigureTestReceiver));
            _outboxToMessageQueuePublisher = new OutboxToMessageQueuePublisher(Repository, messageBusEventPublisher, new EventDescriptorMapper());
        }

        [Test]
        public void Publishes_messages_from_outbox()
        {
            Repository.Add(new TabOpened
            {
                TabId = 123,
                TableNumber = 234,
                Waiter = "John"
            });
            WriteSession.Flush();

            _outboxToMessageQueuePublisher.PublishQueuedMessages();
            const int oneSecond = 1000; // i.e. 1000 ms.
            Thread.Sleep(oneSecond);

            Assert.That(_messagesPublished, Is.EqualTo(1));
        }

        private void ConfigureTestReceiver(IRabbitMqBusFactoryConfigurator sbc, IRabbitMqHost host)
        {
            sbc.ReceiveEndpoint(host, _publishLocation, ep =>
            {
                ep.Handler<IEvent>(context =>
                {
                    var messages = string.Join("\n", context.Message);
                    _messagesPublished++;
                    return Console.Out.WriteLineAsync($"Received: {messages}");
                });
            });
        }

    }
}
