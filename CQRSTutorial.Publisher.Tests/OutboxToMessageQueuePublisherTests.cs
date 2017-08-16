using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CQRSTutorial.Core;
using CQRSTutorial.DAL;
using CQRSTutorial.DAL.Tests.Common;
using CQRSTutorial.Infrastructure;
using CQRSTutorial.Tests.Common;
using log4net;
using MassTransit;
using MassTransit.RabbitMqTransport;
using NHibernate;
using NSubstitute;
using NUnit.Framework;

namespace CQRSTutorial.Publisher.Tests
{
    [TestFixture, Ignore("Very flaky - need to revisit how we test this")] // TODO: write more reliable tests that don't get tripped up by other tests.
    public class OutboxToMessageQueuePublisherTests
    {
        private ISessionFactory _sessionFactory;
        private readonly EventToPublishMapper _eventToPublishMapper = new EventToPublishMapper(typeof(TestEvent).Assembly);
        private static readonly string Queue1 = $"{nameof(OutboxToMessageQueuePublisherTests)}_queue1";
        private static readonly string Queue2 = $"{nameof(OutboxToMessageQueuePublisherTests)}_queue2";
        private static readonly string Queue3 = $"{nameof(OutboxToMessageQueuePublisherTests)}_queue3";
        private static readonly Guid MessageId1 = new Guid("837505FF-F7C5-4F51-A6C6-F76A4980DF36");
        private static readonly Guid MessageId2 = new Guid("533DD041-5FC0-4C19-A574-0AD17C61639E");
        private static readonly Guid AggregateId1 = new Guid("97288F2F-E4FB-40FB-A848-5BBF824F1B38");
        private static readonly Guid AggregateId2 = new Guid("45BE9A71-AEE0-44D8-B31F-33C9F6417377");
        private TestEvent _testEvent1;
        private TestEvent _testEvent2;
        private IEventToPublishRepository _eventToPublishRepository;
        private IOutboxToMessageQueuePublisherConfiguration _outboxToMessageQueuePublisherConfiguration;
        private readonly ILog _logger = LogManager.GetLogger(typeof(OutboxToMessageQueuePublisherTests));

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
            _outboxToMessageQueuePublisherConfiguration = Substitute.For<IOutboxToMessageQueuePublisherConfiguration>();
        }

        [Test]
        public void Publishes_messages_from_outbox_until_there_are_no_more_events_to_publish()
        {
            var batchSize = 1;
            AssumingMessageHasBeenQueuedForPublishing(batchSize, _testEvent1, _testEvent2);
            var messagesPublished = 0;
            const int expectedNumberOfMessagesPublished = 2;
            var manualResetEvent = new ManualResetEvent(false);
            var messageBusEventPublisher = CreateMessageBusEventPublisher(Queue1,
                () =>
                {
                    messagesPublished++;
                    if (ReadyToReturnControlToMainTestThreadToAssertMessageCount(messagesPublished, expectedNumberOfMessagesPublished))
                    {
                        ReturnControlToMainTestThread(manualResetEvent);
                    }
                });

            _outboxToMessageQueuePublisherConfiguration.BatchSize.Returns(batchSize);

            WhenQueuedMessageGetsPublished(messageBusEventPublisher, manualResetEvent);

            Assert.That(messagesPublished, Is.EqualTo(expectedNumberOfMessagesPublished));
        }

        [Test]
        public void Deletes_published_messages_from_outbox()
        {
            var batchSize = 123;
            AssumingMessageHasBeenQueuedForPublishing(batchSize, _testEvent2);
            var manualResetEvent = new ManualResetEvent(false);
            var messageBusEventPublisher = CreateMessageBusEventPublisher(Queue2, () => ReturnControlToMainTestThread(manualResetEvent));
            _outboxToMessageQueuePublisherConfiguration.BatchSize.Returns(batchSize);

            WhenQueuedMessageGetsPublished(messageBusEventPublisher, manualResetEvent);

            _eventToPublishRepository.Received(1).Delete(Arg.Is<EventToPublish>(x => x.Id == MessageId2));
        }

        [Test]
        public void Uses_batch_size_from_configuration()
        {
            var batchSize = 123;
            AssumingNothingHasBeenQueuedForPublishing();
            var manualResetEvent = new ManualResetEvent(false);
            var messageBusEventPublisher = CreateMessageBusEventPublisher(Queue3, () => ReturnControlToMainTestThread(manualResetEvent));
            _outboxToMessageQueuePublisherConfiguration.BatchSize.Returns(batchSize);

            WhenQueuedMessageGetsPublished(messageBusEventPublisher, manualResetEvent);

            _eventToPublishRepository.Received(1).GetEventsAwaitingPublishing(Arg.Is(batchSize));
        }

        private void AssumingNothingHasBeenQueuedForPublishing()
        {
            _eventToPublishRepository
                .GetEventsAwaitingPublishing(Arg.Any<int>())
                .Returns(new EventsToPublishResult
                {
                    EventsToPublish = new List<EventToPublish>()
                });
        }

        private void AssumingMessageHasBeenQueuedForPublishing(int batchSize, params IEvent[] events)
        {
            var eventsToPublishResults = new List<EventsToPublishResult>();
            int index = 0;
            EventsToPublishResult currentEventsToPublishResult = null;
            foreach (var @event in events)
            {
                if (index % batchSize == 0)
                {
                    currentEventsToPublishResult = new EventsToPublishResult
                    {
                        EventsToPublish = new List<EventToPublish>(),
                        TotalNumberOfEventsToPublish = events.Length - index
                    };

                    eventsToPublishResults.Add(currentEventsToPublishResult);
                }

                var eventToPublish = _eventToPublishMapper.MapToEventToPublish(@event);
                currentEventsToPublishResult.EventsToPublish.Add(eventToPublish);
                index++;
            }

            _eventToPublishRepository
                .GetEventsAwaitingPublishing(Arg.Any<int>())
                .Returns(eventsToPublishResults.First(), eventsToPublishResults.Skip(1).ToArray());
        }

        private void WhenQueuedMessageGetsPublished(
            MessageBusEventPublisher messageBusEventPublisher,
            ManualResetEvent manualResetEvent)
        {
            var outboxToMessageQueuePublisher = CreateOutboxToMessageQueuePublisher(messageBusEventPublisher, _outboxToMessageQueuePublisherConfiguration);
            outboxToMessageQueuePublisher.PublishQueuedMessages();
            var isSignalled = manualResetEvent.WaitOne(3000);
            _logger.Debug(isSignalled ? "Another thread unblocked this thread." : "Timeout");
        }

        private OutboxToMessageQueuePublisher CreateOutboxToMessageQueuePublisher(
            MessageBusEventPublisher messageBusEventPublisher,
            IOutboxToMessageQueuePublisherConfiguration outboxToMessageQueuePublisherConfiguration)
        {
            var outboxToMessageQueuePublisher = new OutboxToMessageQueuePublisher(
                _eventToPublishRepository,
                messageBusEventPublisher,
                _eventToPublishMapper,
                new NHibernateUnitOfWorkFactory(_sessionFactory),
                outboxToMessageQueuePublisherConfiguration,
                _logger);
            return outboxToMessageQueuePublisher;
        }

        private MessageBusEventPublisher CreateMessageBusEventPublisher(string queueName, Action onMessagePublished)
        {
            return new MessageBusEventPublisher(
                new MessageBusFactory(new EnvironmentVariableMessageBusConfiguration(),
                (sbc, host) => ConfigureTestReceiver(sbc, host, queueName,
                onMessagePublished)));
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

        private bool ReadyToReturnControlToMainTestThreadToAssertMessageCount(int messagesPublished, int expectedNumberOfMessagesPublished)
        {
            return messagesPublished == expectedNumberOfMessagesPublished;
        }

        private void ReturnControlToMainTestThread(ManualResetEvent manualResetEvent)
        {
            manualResetEvent.Set();
        }
    }
}
