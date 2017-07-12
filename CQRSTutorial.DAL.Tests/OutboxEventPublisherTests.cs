using System;
using System.Collections.Generic;
using System.Data;
using Cafe.Domain;
using Cafe.Domain.Events;
using CQRSTutorial.Core;
using NUnit.Framework;

namespace CQRSTutorial.DAL.Tests
{
    [TestFixture]
    public class OutboxEventPublisherTests
    {
        private const int TabId = 321;
        private OutboxEventPublisher _outboxEventPublisher;
        private readonly int _tableNumber = 123;
        private readonly string _waiter = "John";
        private TabOpened _tabOpened;
        private DrinksOrdered _drinksOrdered;
        private SqlExecutor _sqlExecutor;
        private readonly string _drinkDescription = "Coca Cola";
        private readonly int _drinkMenuNumber = 123;
        private readonly decimal _drinkPrice = 2.5m;
        private EventRepositoryDecorator _eventRepositoryDecorator;
        private EventRepositoryDecorator _eventStoreDecorator;
        private const string EventsToPublishTableName = "dbo.EventsToPublish";
        private const string EventStoreTableName = "dbo.Events";

        [SetUp]
        public void SetUp()
        {
            _sqlExecutor = new SqlExecutor();
            _eventRepositoryDecorator = CreateEventRepositoryThatCanSimulateSqlExceptions(new EventRepository(SessionFactory.ReadInstance, IsolationLevel.ReadCommitted, new TestPublishConfiguration("some.rabbitmq.topic.*"), new EventToPublishMapper()));
            _eventStoreDecorator = CreateEventRepositoryThatCanSimulateSqlExceptions(new EventStore(SessionFactory.ReadInstance, IsolationLevel.ReadCommitted, new EventMapper()));
            _outboxEventPublisher = new OutboxEventPublisher(
                new NHibernateUnitOfWorkFactory(SessionFactory.WriteInstance),
                _eventRepositoryDecorator,
                _eventStoreDecorator
            );

            _tabOpened = new TabOpened
            {
                AggregateId = TabId,
                TableNumber = _tableNumber,
                Waiter = _waiter
            };
            _drinksOrdered = new DrinksOrdered
            {
                AggregateId = TabId,
                Items = new List<OrderedItem>
                    {
                        new OrderedItem
                        {
                            Description = _drinkDescription,
                            IsDrink = true,
                            MenuNumber = _drinkMenuNumber,
                            Price = _drinkPrice
                        }
                    }
            };
        }

        [Test]
        public void Published_events_get_saved_to_database()
        {
            try
            {
                _outboxEventPublisher.Publish(new[] { _tabOpened });

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
        public void No_events_get_saved_to_database_if_any_fail_to_save()
        {
            try
            {
                AssumingSecondSaveCausesException();

                _outboxEventPublisher.Publish(new IEvent[] { _tabOpened, _drinksOrdered });

                AssertThatNoEventsSavedToEventsToPublishTable(_tabOpened.Id,_drinksOrdered.Id);
                AssertThatNoEventsSavedToEventStore(_tabOpened.Id,_drinksOrdered.Id);
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
            var sql = $"SELECT COUNT(*) FROM {tableName} WHERE Id = {_tabOpened.Id}";
            Console.WriteLine(sql);
            var numberOfEventsInserted =
                _sqlExecutor.ExecuteScalar(sql);
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
                _sqlExecutor.ExecuteScalar($"SELECT COUNT(*) FROM {tableName} WHERE Id IN ({commaSeparatedIds})");
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
            _sqlExecutor.ExecuteNonQuery($"DELETE FROM {tableName} WHERE Id = {_tabOpened.Id}");
        }
    }
}