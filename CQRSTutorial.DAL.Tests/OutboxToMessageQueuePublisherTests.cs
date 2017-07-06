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
    public class OutboxToMessageQueuePublisherTests
    {
        private OutboxToMessageQueuePublisher _outboxToMessageQueuePublisher;
        private IPublishConfiguration _publishConfiguration;
        private readonly string _publishLocation = $"{nameof(OutboxToMessageQueuePublisherTests)}_queue";
        private int _messagesPublished;
        private EventRepository _eventRepository;
        private EventDescriptorRepository _eventDescriptorRepository;
        private ISession _writeSession;
        private readonly SqlExecutor _sqlExecutor = new SqlExecutor();

        [SetUp]
        public void SetUp()
        {
            _publishConfiguration = new TestPublishConfiguration(_publishLocation);
            _writeSession = SessionFactory.WriteInstance.OpenSession();
            _writeSession.BeginTransaction();
            var readSessionFactory = SessionFactory.ReadInstance;
            var isolationLevel = IsolationLevel.ReadCommitted;
            _eventRepository = new EventRepository(
                readSessionFactory,
                isolationLevel,
                _publishConfiguration,
                new EventDescriptorMapper())
            {
                UnitOfWork = new NHibernateUnitOfWork(_writeSession)
            };
            _eventDescriptorRepository = new EventDescriptorRepository(readSessionFactory, isolationLevel);
            var messageBusEventPublisher = new MessageBusEventPublisher(new MessageBusFactory(new EnvironmentVariableMessageBusConfiguration(), ConfigureTestReceiver));
            _outboxToMessageQueuePublisher = new OutboxToMessageQueuePublisher(_eventDescriptorRepository, messageBusEventPublisher, new EventDescriptorMapper());
        }

        [Test]
        public void Publishes_messages_from_outbox()
        {
            var tabOpened = new TabOpened
            {
                TabId = 123,
                TableNumber = 234,
                Waiter = "John"
            };

            try
            {
                _eventRepository.Add(tabOpened);
                _writeSession.Flush();
                _writeSession.Transaction.Commit();

                _outboxToMessageQueuePublisher.PublishQueuedMessages();
                const int oneSecond = 1000; // i.e. 1000 ms.
                Thread.Sleep(oneSecond);

                Assert.That(_messagesPublished, Is.EqualTo(1));
            }
            finally
            {
                DeleteNewlyInsertedRow(tabOpened.Id);
            }
        }

        private void ConfigureTestReceiver(IRabbitMqBusFactoryConfigurator sbc, IRabbitMqHost host)
        {
            sbc.ReceiveEndpoint(host, _publishLocation, ep =>
            {
                ep.Handler<IEvent>(context =>
                {
                    var messages = string.Join("\n", context.Message);
                    _messagesPublished++;
                    return Console.Out.WriteLineAsync($"Received: [Id:{context.Message.Id}] {messages}");
                });
            });
        }

        private void DeleteNewlyInsertedRow(int id)
        {
            _sqlExecutor.ExecuteNonQuery($"DELETE FROM dbo.EventsToPublish WHERE Id = {id}");
        }
    }
}
