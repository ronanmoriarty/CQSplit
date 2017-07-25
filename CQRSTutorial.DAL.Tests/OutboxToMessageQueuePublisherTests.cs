using System;
using System.Data;
using System.Reflection;
using System.Threading;
using CQRSTutorial.Core;
using CQRSTutorial.DAL.Tests.Common;
using CQRSTutorial.Infrastructure;
using CQRSTutorial.Publisher;
using MassTransit;
using MassTransit.RabbitMqTransport;
using NUnit.Framework;

namespace CQRSTutorial.DAL.Tests
{
    [TestFixture]
    public class OutboxToMessageQueuePublisherTests
    {
        private const string Reason = @"These tests are very flaky. These tests trip over each other as they don't take account of the fact that
            MassTransit uses fanout exchanges, and so the queues that were intended to be test-specific receive messages created by both tests.
            These tests are completely unreadable too but will refactor after I figure out how I want to test with fanout exchanges.";
        private readonly SqlExecutor _sqlExecutor = new SqlExecutor();

        [SetUp]
        public void SetUp()
        {
            CleanUpBeforeRunningTests();
        }

        [Test, Ignore(Reason)]
        public void Publishes_messages_from_outbox()
        {
            var testEvent = new TestEvent
            {
                AggregateId = 123,
                IntProperty = 234,
                StringProperty = "John"
            };

            try
            {
                var isolationLevel = IsolationLevel.ReadCommitted;
                var readSessionFactory = SessionFactory.ReadInstance;
                var publishLocation = $"{nameof(OutboxToMessageQueuePublisherTests)}_queue1";
                var eventToPublishMapper = new EventToPublishMapper(Assembly.GetExecutingAssembly());
                using (var writeSession = SessionFactory.WriteInstance.OpenSession())
                {
                    writeSession.BeginTransaction();
                    var nHibernateUnitOfWork = new NHibernateUnitOfWork(writeSession);
                    var publishConfiguration = new TestPublishConfiguration(publishLocation);
                    var eventToPublishRepository = new EventToPublishRepository(
                        readSessionFactory,
                        publishConfiguration,
                        eventToPublishMapper)
                    {
                        UnitOfWork = nHibernateUnitOfWork
                    };
                    eventToPublishRepository.Add(testEvent);
                    writeSession.Flush();
                    writeSession.Transaction.Commit();
                }

                var messagesPublished = 0;
                var messageBusEventPublisher = new MessageBusEventPublisher(new MessageBusFactory(new EnvironmentVariableMessageBusConfiguration(), (sbc, host) => ConfigureTestReceiver(sbc, host, publishLocation, () => messagesPublished++)));
                using (var writeSession = SessionFactory.WriteInstance.OpenSession())
                {
                    var eventToPublishRepository = new EventToPublishRepository(readSessionFactory, null, eventToPublishMapper)
                    {
                        UnitOfWork = new NHibernateUnitOfWork(writeSession)
                    };
                    var outboxToMessageQueuePublisher = new OutboxToMessageQueuePublisher(eventToPublishRepository, messageBusEventPublisher, eventToPublishMapper);
                    outboxToMessageQueuePublisher.PublishQueuedMessages();
                    const int oneSecond = 1000; // i.e. 1000 ms.
                    Thread.Sleep(oneSecond);

                    Assert.That(messagesPublished, Is.EqualTo(1));
                }
            }
            finally
            {
                DeleteNewlyInsertedRow(testEvent.Id);
            }
        }

        [Test, Ignore(Reason)]
        public void Deletes_published_messages_from_outbox()
        {
            var readSessionFactory = SessionFactory.ReadInstance;
            var isolationLevel = IsolationLevel.ReadCommitted;
            int tabOpenedId;
            var publishLocation = $"{nameof(OutboxToMessageQueuePublisherTests)}_queue2";
            var eventToPublishMapper = new EventToPublishMapper(Assembly.GetExecutingAssembly());
            using (var writeSession = SessionFactory.WriteInstance.OpenSession())
            {
                writeSession.BeginTransaction();
                var nHibernateUnitOfWork = new NHibernateUnitOfWork(writeSession);
                var publishConfiguration = new TestPublishConfiguration(publishLocation);
                var eventToPublishRepository = new EventToPublishRepository(
                    readSessionFactory,
                    publishConfiguration,
                    eventToPublishMapper)
                {
                    UnitOfWork = nHibernateUnitOfWork
                };

                var testEvent = new TestEvent
                {
                    AggregateId = 345,
                    IntProperty = 456,
                    StringProperty = "Mary"
                };

                eventToPublishRepository.Add(testEvent);
                writeSession.Flush();
                writeSession.Transaction.Commit();
                tabOpenedId = testEvent.Id;
            }

            using (var writeSession = SessionFactory.WriteInstance.OpenSession())
            {
                using (var transaction = writeSession.BeginTransaction())
                {
                    var eventToPublishRepository = new EventToPublishRepository(readSessionFactory, null, eventToPublishMapper)
                    {
                        UnitOfWork = new NHibernateUnitOfWork(writeSession)
                    };
                    var messageBusEventPublisher = new MessageBusEventPublisher(
                        new MessageBusFactory(new EnvironmentVariableMessageBusConfiguration(),
                        (sbc, host) => ConfigureTestReceiver(sbc, host, publishLocation))
                    );
                    var outboxToMessageQueuePublisher = new OutboxToMessageQueuePublisher(eventToPublishRepository, messageBusEventPublisher, eventToPublishMapper);

                    outboxToMessageQueuePublisher.PublishQueuedMessages();
                    transaction.Commit();
                }

                const int oneSecond = 1000; // i.e. 1000 ms.
                Thread.Sleep(oneSecond);

                var numberOfEvents = _sqlExecutor.ExecuteScalar<int>($"SELECT COUNT(*) FROM dbo.EventsToPublish WHERE Id = {tabOpenedId}");
                Assert.That(numberOfEvents, Is.EqualTo(0));
            }
        }

        private void CleanUpBeforeRunningTests()
        {
            _sqlExecutor.ExecuteNonQuery(
                $"DELETE FROM dbo.EventsToPublish WHERE PublishTo IN ('{nameof(OutboxToMessageQueuePublisherTests)}_queue1','{nameof(OutboxToMessageQueuePublisherTests)}_queue2')");
        }

        private void ConfigureTestReceiver(IRabbitMqBusFactoryConfigurator sbc, IRabbitMqHost host, string publishLocation, Action onEventHandled = null)
        {
            sbc.ReceiveEndpoint(host, publishLocation, ep =>
            {
                ep.Handler<IEvent>(context =>
                {
                    var messages = string.Join("\n", context.Message);
                    onEventHandled?.Invoke();
                    return Console.Out.WriteLineAsync($"Received: [Id:{context.Message.Id}] {messages} on queue [{publishLocation}]");
                });
            });
        }

        private void DeleteNewlyInsertedRow(int id)
        {
            _sqlExecutor.ExecuteNonQuery($"DELETE FROM dbo.EventsToPublish WHERE Id = {id}");
        }
    }
}
