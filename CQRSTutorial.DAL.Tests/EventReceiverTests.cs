using System;
using System.Data;
using System.Reflection;
using CQRSTutorial.Core;
using CQRSTutorial.DAL.Tests.Common;
using NUnit.Framework;

namespace CQRSTutorial.DAL.Tests
{
    [TestFixture]
    public class EventReceiverTests
    {
        private const int AggregateId = 321;
        private EventReceiver _eventReceiver;
        private TestEvent _testEvent;
        private TestEvent2 _testEvent2;
        private SqlExecutor _sqlExecutor;
        private EventRepositoryDecorator _eventRepositoryDecorator;
        private IEventStore _eventStore;
        private const string EventsToPublishTableName = "dbo.EventsToPublish";
        private const string EventStoreTableName = "dbo.Events";

        [SetUp]
        public void SetUp()
        {
            _sqlExecutor = new SqlExecutor();
            _eventRepositoryDecorator = CreateEventRepositoryThatCanSimulateSqlExceptions(new EventToPublishRepository(SessionFactory.ReadInstance, IsolationLevel.ReadCommitted, new TestPublishConfiguration("some.rabbitmq.topic.*"), new EventToPublishMapper()));
            _eventStore = new EventStore(SessionFactory.ReadInstance, IsolationLevel.ReadCommitted, new EventMapper(Assembly.GetExecutingAssembly()));
            _eventReceiver = new EventReceiver(
                new NHibernateUnitOfWorkFactory(SessionFactory.WriteInstance),
                _eventStore,
                _eventRepositoryDecorator);

            _testEvent = new TestEvent
            {
                AggregateId = AggregateId
            };
            _testEvent2 = new TestEvent2
            {
                AggregateId = AggregateId
            };
        }

        [Test]
        public void Received_events_get_saved_to_database_for_publishing()
        {
            try
            {
                _eventReceiver.Receive(new[] { _testEvent });

                AssertThatEventSavedToEventsToPublishTable();
                AssertThatEventSavedToEventStore();
            }
            finally
            {
                DeleteNewlyInsertedTabOpenedEventFromEventsToPublishTable();
                DeleteNewlyInsertedTabOpenedEventFromEventStore();
            }
        }

        [Test]
        public void No_events_get_saved_to_database_for_publishing_if_any_fail_to_save()
        {
            try
            {
                AssumingSecondSaveCausesException();

                _eventReceiver.Receive(new IEvent[] { _testEvent, _testEvent2 });

                AssertThatNoEventsSavedToEventsToPublishTable(_testEvent.Id,_testEvent2.Id);
                AssertThatNoEventsSavedToEventStore(_testEvent.Id,_testEvent2.Id);
            }
            finally
            {
                DeleteNewlyInsertedTabOpenedEventFromEventsToPublishTable();
                DeleteNewlyInsertedTabOpenedEventFromEventStore();
            }
        }

        private EventRepositoryDecorator CreateEventRepositoryThatCanSimulateSqlExceptions(IEventRepository eventRepositoryToWrap)
        {
            return new EventRepositoryDecorator(eventRepositoryToWrap);
        }

        private void AssumingSecondSaveCausesException()
        {
            int numberOfEventsAdded = 0;
            _eventRepositoryDecorator.OnBeforeAdding = @event =>
            {
                numberOfEventsAdded++;
                if (numberOfEventsAdded == 2)
                {
                    throw new Exception("Simulating Sql error on addition of second event.");
                }
            };
        }

        private void AssertThatEventSavedToEventsToPublishTable()
        {
            AssertThatEventSavedToTable(EventsToPublishTableName);
        }

        private void AssertThatEventSavedToEventStore()
        {
            AssertThatEventSavedToTable(EventStoreTableName);
        }

        private void AssertThatEventSavedToTable(string tableName)
        {
            var sql = $"SELECT COUNT(*) FROM {tableName} WHERE Id = {_testEvent.Id}";
            Console.WriteLine(sql);
            var numberOfEventsInserted =
                _sqlExecutor.ExecuteScalar<int>(sql);
            Assert.That(numberOfEventsInserted, Is.EqualTo(1));
        }

        private void AssertThatNoEventsSavedToEventsToPublishTable(params int[] ids)
        {
            AssertThatNoEventsSavedToTable(ids, EventsToPublishTableName);
        }

        private void AssertThatNoEventsSavedToEventStore(params int[] ids)
        {
            AssertThatNoEventsSavedToTable(ids, EventStoreTableName);
        }

        private void AssertThatNoEventsSavedToTable(int[] ids, string tableName)
        {
            var commaSeparatedIds = string.Join(",", ids);
            var numberOfEventsInserted =
                _sqlExecutor.ExecuteScalar<int>($"SELECT COUNT(*) FROM {tableName} WHERE Id IN ({commaSeparatedIds})");
            Assert.That(numberOfEventsInserted, Is.EqualTo(0));
        }

        private void DeleteNewlyInsertedTabOpenedEventFromEventsToPublishTable()
        {
            DeleteNewlyInsertedEventFromTable(EventsToPublishTableName);
        }

        private void DeleteNewlyInsertedTabOpenedEventFromEventStore()
        {
            DeleteNewlyInsertedEventFromTable(EventStoreTableName);
        }

        private void DeleteNewlyInsertedEventFromTable(string tableName)
        {
            _sqlExecutor.ExecuteNonQuery($"DELETE FROM {tableName} WHERE Id = {_testEvent.Id}");
        }
    }
}