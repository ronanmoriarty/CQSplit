using System;
using System.Collections.Generic;
using System.Threading;
using CQRSTutorial.Core;
using CQRSTutorial.DAL;
using CQRSTutorial.DAL.Tests;
using CQRSTutorial.DAL.Tests.Common;
using CQRSTutorial.Infrastructure;
using CQRSTutorial.Tests.Common;
using MassTransit;
using MassTransit.RabbitMqTransport;
using NHibernate;
using NSubstitute;
using NUnit.Framework;

namespace CQRSTutorial.Publisher.Tests
{
    [TestFixture]
    public class OutboxToMessageQueuePublisherTests
    {
        private readonly SqlExecutor _sqlExecutor = new SqlExecutor(WriteModelConnectionStringProviderFactory.Instance);
        private ISessionFactory _sessionFactory;
        private readonly EventToPublishMapper _eventToPublishMapper = new EventToPublishMapper(typeof(TestEvent).Assembly);
        private static readonly string Queue1 = $"{nameof(OutboxToMessageQueuePublisherTests)}_queue1";
        private static readonly string Queue2 = $"{nameof(OutboxToMessageQueuePublisherTests)}_queue2";
        private static readonly string Queue3 = $"{nameof(OutboxToMessageQueuePublisherTests)}_queue3";
        private static readonly Guid MessageId1 = new Guid("837505FF-F7C5-4F51-A6C6-F76A4980DF36");
        private static readonly Guid MessageId2 = new Guid("533DD041-5FC0-4C19-A574-0AD17C61639E");
        private static readonly Guid AggregateId1 = new Guid("97288F2F-E4FB-40FB-A848-5BBF824F1B38");
        private static readonly Guid AggregateId2 = new Guid("45BE9A71-AEE0-44D8-B31F-33C9F6417377");
        private readonly ManualResetEvent _manualResetEvent = new ManualResetEvent(false);
        private TestEvent _testEvent1;
        private TestEvent _testEvent2;
        private IEventToPublishRepository _eventToPublishRepository;
        private const int BatchSize = 123;

        [SetUp]
        public void SetUp()
        {
            _sessionFactory = SessionFactory.Instance;
            _testEvent1 = new TestEvent
            {
                Id = MessageId1,
                AggregateId = AggregateId1,
                IntProperty = 234,
                StringProperty = "John"
            };
            _testEvent2 = new TestEvent
            {
                Id = MessageId2,
                AggregateId = AggregateId2,
                IntProperty = 456,
                StringProperty = "Mary"
            };
            _eventToPublishRepository = Substitute.For<IEventToPublishRepository>();

            CleanUpBeforeRunningTests();
        }

        [Test]
        public void Publishes_messages_from_outbox()
        {
            AssumingMessageHasBeenQueuedForPublishing(_testEvent1);

            var messagesPublished = 0;
            var messageBusEventPublisher = CreateMessageBusEventPublisher(Queue1,
                () =>
                {
                    messagesPublished++;
                    _manualResetEvent.Set();
                });

            WhenQueuedMessageGetsPublished(messageBusEventPublisher, new OutboxToMessageQueuePublisherConfiguration());

            Assert.That(messagesPublished, Is.EqualTo(1));
        }

        [Test]
        public void Deletes_published_messages_from_outbox()
        {
            AssumingMessageHasBeenQueuedForPublishing(_testEvent2);

            var messageBusEventPublisher = CreateMessageBusEventPublisher(Queue2, () => _manualResetEvent.Set());

            WhenQueuedMessageGetsPublished(messageBusEventPublisher, new OutboxToMessageQueuePublisherConfiguration());

            var numberOfEvents = _sqlExecutor.ExecuteScalar<int>($"SELECT COUNT(*) FROM dbo.EventsToPublish WHERE Id = '{MessageId2}'");
            Assert.That(numberOfEvents, Is.EqualTo(0));
        }

        [Test]
        public void Uses_batch_size_from_configuration()
        {
            var messageBusEventPublisher = CreateMessageBusEventPublisher(Queue3, () => _manualResetEvent.Set());
            var outboxToMessageQueuePublisherConfiguration = Substitute.For<IOutboxToMessageQueuePublisherConfiguration>();
            outboxToMessageQueuePublisherConfiguration.BatchSize.Returns(BatchSize);

            WhenQueuedMessageGetsPublished(messageBusEventPublisher, outboxToMessageQueuePublisherConfiguration);

            _eventToPublishRepository.Received(1).GetEventsAwaitingPublishing(Arg.Is(BatchSize));
        }

        private void AssumingMessageHasBeenQueuedForPublishing(IEvent testEvent)
        {
            var eventToPublish = _eventToPublishMapper.MapToEventToPublish(testEvent);
            _eventToPublishRepository
                .GetEventsAwaitingPublishing(Arg.Any<int>())
                .Returns(new List<EventToPublish> { eventToPublish });
        }

        private void WhenQueuedMessageGetsPublished(MessageBusEventPublisher messageBusEventPublisher,
            IOutboxToMessageQueuePublisherConfiguration outboxToMessageQueuePublisherConfiguration)
        {
            var outboxToMessageQueuePublisher = CreateOutboxToMessageQueuePublisher(messageBusEventPublisher, outboxToMessageQueuePublisherConfiguration);
            outboxToMessageQueuePublisher.PublishQueuedMessages();
            var isSignalled = _manualResetEvent.WaitOne(3000);
            Console.WriteLine(isSignalled ? "Another thread unblocked this thread." : "Timeout");
        }

        private OutboxToMessageQueuePublisher CreateOutboxToMessageQueuePublisher(MessageBusEventPublisher messageBusEventPublisher,
            IOutboxToMessageQueuePublisherConfiguration outboxToMessageQueuePublisherConfiguration)
        {
            var outboxToMessageQueuePublisher = new OutboxToMessageQueuePublisher(
                _eventToPublishRepository,
                messageBusEventPublisher,
                _eventToPublishMapper,
                () => new NHibernateUnitOfWork(_sessionFactory.OpenSession()),
                outboxToMessageQueuePublisherConfiguration);
            return outboxToMessageQueuePublisher;
        }

        private MessageBusEventPublisher CreateMessageBusEventPublisher(string queueName, Action onMessagePublished)
        {
            return new MessageBusEventPublisher(
                new MessageBusFactory(new EnvironmentVariableMessageBusConfiguration(),
                (sbc, host) => ConfigureTestReceiver(sbc, host, queueName,
                onMessagePublished)));
        }

        private void CleanUpBeforeRunningTests()
        {
            _sqlExecutor.ExecuteNonQuery($"DELETE FROM dbo.EventsToPublish WHERE Id IN ('{MessageId1}','{MessageId2}')");
        }

        private void ConfigureTestReceiver(IRabbitMqBusFactoryConfigurator sbc, IRabbitMqHost host, string queueName, Action onEventHandled)
        {
            sbc.ReceiveEndpoint(host, queueName, ep =>
            {
                ep.Handler<IEvent>(context =>
                {
                    if (context.Message.Id == MessageId1 ||
                        context.Message.Id == MessageId2)
                    {
                        onEventHandled?.Invoke();
                        return Console.Out.WriteLineAsync($"Received: [Id:{context.Message.Id}; Type:{context.Message.GetType()}; Queue:{queueName}]");
                    }

                    return Console.Out.WriteLineAsync($"Received message queued from another test fixture: [Id:{context.Message.Id}; Type:{context.Message.GetType()}; Queue:{queueName}]. Message will be disregarded for the purpose of this test.");
                });
            });
        }

        //internal class TestMessage1 : IEvent
        //{
        //    public Guid Id { get; set; }
        //    public Guid AggregateId { get; set; }
        //    public Guid CommandId { get; set; }
        //    public int IntProperty { get; set; }
        //    public string StringProperty { get; set; }
        //}

        //internal class TestMessage2 : IEvent
        //{
        //    public Guid Id { get; set; }
        //    public Guid AggregateId { get; set; }
        //    public Guid CommandId { get; set; }
        //    public int IntProperty { get; set; }
        //    public string StringProperty { get; set; }
        //}
    }
}
