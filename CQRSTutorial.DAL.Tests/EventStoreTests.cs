using System.Collections.Generic;
using System.Data;
using System.Linq;
using Cafe.Domain;
using Cafe.Domain.Events;
using CQRSTutorial.Core;
using CQRSTutorial.DAL.Tests.Common;
using NHibernate;
using NUnit.Framework;

namespace CQRSTutorial.DAL.Tests
{
    [TestFixture, Category(TestConstants.Integration)]
    public class EventStoreTests
        : InsertAndReadTest<EventStore, Event>
    {
        private IEvent _retrievedEvent;
        private const int AggregateId = 234;

        protected override EventStore CreateRepository(ISessionFactory readSessionFactory, IsolationLevel isolationLevel)
        {
            return new EventStore(readSessionFactory, isolationLevel, new EventMapper());
        }

        [Test]
        public void InsertAndReadTabOpened()
        {
            const string waiter = "John";
            const int tableNumber = 123;

            var tabOpened = new TabOpened
            {
                AggregateId = AggregateId,
                TableNumber = tableNumber,
                Waiter = waiter
            };

            InsertAndRead(tabOpened);

            Assert.That(_retrievedEvent is TabOpened);
            var retrievedTabOpenedEvent = (TabOpened) _retrievedEvent;
            Assert.That(retrievedTabOpenedEvent.Id, Is.Not.Null);
            Assert.That(retrievedTabOpenedEvent.AggregateId, Is.EqualTo(tabOpened.AggregateId));
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
            Assert.That(retrievedFoodOrderedEvent.Id, Is.Not.Null);
            Assert.That(retrievedFoodOrderedEvent.AggregateId, Is.EqualTo(foodOrdered.AggregateId));
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
            Assert.That(retrievedDrinkOrderedEvent.Id, Is.Not.Null);
            Assert.That(retrievedDrinkOrderedEvent.AggregateId, Is.EqualTo(drinkOrdered.AggregateId));

            Assert.That(retrievedDrinkOrderedEvent.Items.Count, Is.EqualTo(1));
            var drinkOrderedItem = retrievedDrinkOrderedEvent.Items.Single();
            Assert.That(drinkOrderedItem.Description, Is.EqualTo(drinkDescription));
            Assert.That(drinkOrderedItem.IsDrink, Is.True);
            Assert.That(drinkOrderedItem.MenuNumber, Is.EqualTo(drinkMenuNumber));
            Assert.That(drinkOrderedItem.Price, Is.EqualTo(drinkPrice));
        }

        private void InsertAndRead(IEvent @event)
        {
            Repository.Add(@event);
            WriteSession.Flush();
            _retrievedEvent = Repository.Read(@event.Id);
        }
    }
}