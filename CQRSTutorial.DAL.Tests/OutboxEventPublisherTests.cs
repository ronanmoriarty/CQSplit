using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Cafe.Domain;
using Cafe.Domain.Events;
using CQRSTutorial.Core;
using NUnit.Framework;

namespace CQRSTutorial.DAL.Tests
{
    [TestFixture]
    public class OutboxEventPublisherTests
    {
        protected const int TabId = 321;
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
        private const string EventsToPublishTableName = "dbo.EventsToPublish";

        [SetUp]
        public void SetUp()
        {
            _sqlExecutor = new SqlExecutor();
            _eventRepositoryDecorator = CreateEventRepositoryThatCanSimulateSqlExceptions();
            _outboxEventPublisher = new OutboxEventPublisher(
                SessionFactory.WriteInstance,
                _eventRepositoryDecorator
            );

            _tabOpened = new TabOpened
            {
                TabId = TabId,
                TableNumber = _tableNumber,
                Waiter = _waiter
            };
            _drinksOrdered = new DrinksOrdered
            {
                TabId = TabId,
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

                AssertThatEventSaved();
            }
            finally
            {
                DeleteNewlyInsertedTabOpenedEvent();
            }
        }

        [Test]
        public void No_events_get_saved_to_database_if_any_fail_to_save()
        {
            try
            {
                AssumingSecondSaveCausesException();

                _outboxEventPublisher.Publish(new IEvent[] { _tabOpened, _drinksOrdered });

                AssertThatNoEventsSaved(_tabOpened.Id,_drinksOrdered.Id);
            }
            finally
            {
                DeleteNewlyInsertedTabOpenedEvent();
            }
        }

        private EventRepositoryDecorator CreateEventRepositoryThatCanSimulateSqlExceptions()
        {
            return new EventRepositoryDecorator(new EventRepository(SessionFactory.ReadInstance, IsolationLevel.ReadCommitted, new TestPublishConfiguration("some.rabbitmq.topic.*")));
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

        private void AssertThatEventSaved()
        {
            var numberOfEventsInserted = _sqlExecutor.ExecuteScalar($"SELECT COUNT(*) FROM {EventsToPublishTableName} WHERE Id = '{_tabOpened.Id}'");
            Assert.That(numberOfEventsInserted, Is.EqualTo(1));
        }

        private void AssertThatNoEventsSaved(params int[] ids)
        {
            var commaSeparatedIds = string.Join(",", ids);
            var numberOfEventsInserted = _sqlExecutor.ExecuteScalar($"SELECT COUNT(*) FROM {EventsToPublishTableName} WHERE Id IN ({commaSeparatedIds})");
            Assert.That(numberOfEventsInserted, Is.EqualTo(0));
        }

        private void DeleteNewlyInsertedTabOpenedEvent()
        {
            _sqlExecutor.ExecuteNonQuery($"DELETE FROM {EventsToPublishTableName} WHERE Id = {_tabOpened.Id}");
        }
    }
}