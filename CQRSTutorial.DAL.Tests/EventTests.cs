using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Cafe.Domain;
using Cafe.Domain.Events;
using CQRSTutorial.Core;
using NHibernate;
using NUnit.Framework;

namespace CQRSTutorial.DAL.Tests
{
    [TestFixture, Category(TestConstants.Integration)]
    public class EventTests
        : InsertAndReadTest<EventRepository, EventToPublish>
    {
        private IEvent _retrievedEvent;
        private readonly IPublishConfiguration _publishConfiguration;
        private const string PublishLocation = "some.rabbitmq.topic.*";
        private readonly EventRepository _eventToPublishRepository;
        private readonly SqlExecutor _sqlExecutor;

        public EventTests()
        {
            _eventToPublishRepository = new EventRepository(SessionFactory.ReadInstance, IsolationLevel.ReadUncommitted, null, new EventToPublishMapper());
            _publishConfiguration = new TestPublishConfiguration(PublishLocation);
            _sqlExecutor = new SqlExecutor();
        }

        protected override EventRepository CreateRepository(ISessionFactory readSessionFactory, IsolationLevel isolationLevel)
        {
            return new EventRepository(readSessionFactory, isolationLevel, _publishConfiguration, new EventToPublishMapper());
        }

        [Test]
        public void InsertAndReadTabOpened()
        {
            const string waiter = "John";
            const int tableNumber = 123;

            var tabOpened = new TabOpened
            {
                Id = GetNewId(),
                TableNumber = tableNumber,
                Waiter = waiter
            };

            InsertAndRead(tabOpened);

            Assert.That(_retrievedEvent is TabOpened);
            var retrievedTabOpenedEvent = (TabOpened) _retrievedEvent;
            Assert.That(retrievedTabOpenedEvent.Id, Is.EqualTo(tabOpened.Id));
            Assert.That(retrievedTabOpenedEvent.TableNumber, Is.EqualTo(tableNumber));
            Assert.That(retrievedTabOpenedEvent.Waiter, Is.EqualTo(waiter));
        }

        [Test]
        public void InsertAndReadFoodOrdered()
        {
            const string foodDescription = "Chicken Madras";
            const int foodMenuNumber = 234;
            const decimal foodPrice = 10.5m;

            var foodOrdered = new FoodOrdered
            {
                Id = GetNewId(),
                Items = new List<OrderedItem>
                {
                    new OrderedItem
                    {
                        Description = foodDescription,
                        IsDrink = false,
                        MenuNumber = foodMenuNumber,
                        Price = foodPrice
                    }
                }
            };

            InsertAndRead(foodOrdered);

            Assert.That(_retrievedEvent is FoodOrdered);
            var retrievedFoodOrderedEvent = (FoodOrdered)_retrievedEvent;
            Assert.That(retrievedFoodOrderedEvent.Id, Is.EqualTo(foodOrdered.Id));
            Assert.That(retrievedFoodOrderedEvent.Items.Count, Is.EqualTo(1));
            var foodOrderedItem = retrievedFoodOrderedEvent.Items.Single();
            Assert.That(foodOrderedItem.Description, Is.EqualTo(foodDescription));
            Assert.That(foodOrderedItem.IsDrink, Is.False);
            Assert.That(foodOrderedItem.MenuNumber, Is.EqualTo(foodMenuNumber));
            Assert.That(foodOrderedItem.Price, Is.EqualTo(foodPrice));
        }

        [Test]
        public void InsertAndReadDrinkOrdered()
        {
            const string drinkDescription = "Coca Cola";
            const int drinkMenuNumber = 101;
            const decimal drinkPrice = 2.5m;

            var drinkOrdered = new DrinksOrdered
            {
                Id = GetNewId(),
                Items = new List<OrderedItem>
                {
                    new OrderedItem
                    {
                        Description = drinkDescription,
                        IsDrink = true,
                        MenuNumber = drinkMenuNumber,
                        Price = drinkPrice
                    }
                }
            };

            InsertAndRead(drinkOrdered);

            Assert.That(_retrievedEvent is DrinksOrdered);
            var retrievedDrinkOrderedEvent = (DrinksOrdered)_retrievedEvent;
            Assert.That(retrievedDrinkOrderedEvent.Id, Is.EqualTo(drinkOrdered.Id));
            Assert.That(retrievedDrinkOrderedEvent.Items.Count, Is.EqualTo(1));
            var drinkOrderedItem = retrievedDrinkOrderedEvent.Items.Single();
            Assert.That(drinkOrderedItem.Description, Is.EqualTo(drinkDescription));
            Assert.That(drinkOrderedItem.IsDrink, Is.True);
            Assert.That(drinkOrderedItem.MenuNumber, Is.EqualTo(drinkMenuNumber));
            Assert.That(drinkOrderedItem.Price, Is.EqualTo(drinkPrice));
        }

        [Test]
        public void Event_PublishTo_set_according_to_PublishConfiguration()
        {
            const string waiter = "John";
            const int tableNumber = 123;

            var tabOpened = new TabOpened
            {
                Id = GetNewId(),
                TableNumber = tableNumber,
                Waiter = waiter
            };

            Repository.Add(tabOpened);
            WriteSession.Flush();
            var eventToPublish = _eventToPublishRepository.Get(tabOpened.Id);

            Assert.That(eventToPublish.PublishTo, Is.EqualTo(PublishLocation));
        }

        private int GetNewId()
        {
            var maxValue = _sqlExecutor.ExecuteScalar<int?>("SELECT MAX(Id) FROM dbo.EventsToPublish");
            return maxValue + 1 ?? 1;
        }

        private void InsertAndRead(IEvent @event)
        {
            Repository.Add(@event);
            WriteSession.Flush();
            _retrievedEvent = Repository.Read(@event.Id);
        }
    }

    public class TestPublishConfiguration : IPublishConfiguration
    {
        private readonly string _location;

        public TestPublishConfiguration(string location)
        {
            _location = location;
        }
        public string GetPublishLocationFor(Type typeToPublish)
        {
            return _location;
        }
    }
}