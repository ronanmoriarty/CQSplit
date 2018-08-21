using System;
using System.Linq;
using System.Reflection;
using Cafe.DAL.Common;
using Cafe.DAL.Sql;
using Cafe.DAL.Tests.Common;
using CQSplit.Core;
using CQSplit.DAL;
using NLog;
using NUnit.Framework;
using EventHandler = CQSplit.DAL.EventHandler;

namespace Cafe.DAL.Tests
{
    [TestFixture, Category(TestConstants.Integration)]
    public class EventHandlerTests
    {
        private readonly Guid _aggregateId = new Guid("8E3337B6-7378-445B-BD43-61D56624C10E");
        private EventHandler _eventHandler;
        private TestEvent _testEvent;
        private TestEvent2 _testEvent2;
        private SqlExecutor _sqlExecutor;
        private EventToPublishRepositoryDecorator _eventToPublishRepositoryDecorator;
        private IEventStore _eventStore;
        private const string EventsToPublishTableName = "dbo.EventsToPublish";
        private const string EventStoreTableName = "dbo.Events";
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private const string Id1 = "B3C9CBC3-E09B-4C9E-A331-FA11BC3185F9";
        private const string Id2 = "259C8A26-5BCC-4986-8C15-6BE305195923";

        [SetUp]
        public void SetUp()
        {
            var connectionStringProvider = new ConnectionStringProviderFactory(ConfigurationRoot.Instance).GetConnectionStringProvider();
            _sqlExecutor = new SqlExecutor(connectionStringProvider);
            _sqlExecutor.ExecuteNonQuery($"DELETE FROM dbo.Events WHERE ID IN ('{Id1}','{Id2}')");
            _sqlExecutor.ExecuteNonQuery($"DELETE FROM dbo.EventsToPublish WHERE ID IN ('{Id1}','{Id2}')");
            var eventStoreUnitOfWork = new EventStoreUnitOfWork(connectionStringProvider);
            _eventToPublishRepositoryDecorator = CreateEventToPublishRepositoryThatCanSimulateSqlExceptions(
                new EventToPublishRepository(new EventToPublishSerializer(Assembly.GetExecutingAssembly())
                )
            );
            _eventToPublishRepositoryDecorator.UnitOfWork = eventStoreUnitOfWork;
            _eventStore = new EventRepository(new EventSerializer(Assembly.GetExecutingAssembly()))
            {
                UnitOfWork = eventStoreUnitOfWork
            };

            _eventHandler = new EventHandler(
                new CompositeEventStore(
                    new[]
                    {
                        _eventStore,
                        _eventToPublishRepositoryDecorator
                    }
                )
            );

            _testEvent = new TestEvent
            {
                Id = new Guid(Id1),
                AggregateId = _aggregateId
            };
            _testEvent2 = new TestEvent2
            {
                Id = new Guid(Id2),
                AggregateId = _aggregateId
            };
        }

        [Test]
        public void Received_events_get_saved_to_database_for_publishing()
        {
            try
            {
                _eventHandler.Handle(new[] { _testEvent });

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

                _eventHandler.Handle(new IEvent[] { _testEvent, _testEvent2 });

                AssertThatNoEventsSavedToEventsToPublishTable(_testEvent.Id,_testEvent2.Id);
                AssertThatNoEventsSavedToEventStore(_testEvent.Id,_testEvent2.Id);
            }
            finally
            {
                DeleteNewlyInsertedTabOpenedEventFromEventsToPublishTable();
                DeleteNewlyInsertedTabOpenedEventFromEventStore();
            }
        }

        private EventToPublishRepositoryDecorator CreateEventToPublishRepositoryThatCanSimulateSqlExceptions(EventToPublishRepository innerEventToPublishRepository)
        {
            return new EventToPublishRepositoryDecorator(innerEventToPublishRepository);
        }

        private void AssumingSecondSaveCausesException()
        {
            int numberOfEventsAdded = 0;
            _eventToPublishRepositoryDecorator.OnBeforeAdding = @event =>
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
            var sql = $"SELECT COUNT(*) FROM {tableName} WHERE Id = '{_testEvent.Id}'";
            _logger.Debug(sql);
            var numberOfEventsInserted =
                _sqlExecutor.ExecuteScalar<int>(sql);
            Assert.That(numberOfEventsInserted, Is.EqualTo(1));
        }

        private void AssertThatNoEventsSavedToEventsToPublishTable(params Guid[] ids)
        {
            AssertThatNoEventsSavedToTable(ids, EventsToPublishTableName);
        }

        private void AssertThatNoEventsSavedToEventStore(params Guid[] ids)
        {
            AssertThatNoEventsSavedToTable(ids, EventStoreTableName);
        }

        private void AssertThatNoEventsSavedToTable(Guid[] ids, string tableName)
        {
            var commaSeparatedIdsEnclosedInSingleQuotes = string.Join(",", ids.Select(id => $"'{id}'"));
            var numberOfEventsInserted =
                _sqlExecutor.ExecuteScalar<int>($"SELECT COUNT(*) FROM {tableName} WHERE Id IN ({commaSeparatedIdsEnclosedInSingleQuotes})");
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
            _sqlExecutor.ExecuteNonQuery($"DELETE FROM {tableName} WHERE Id = '{_testEvent.Id}'");
        }
    }
}