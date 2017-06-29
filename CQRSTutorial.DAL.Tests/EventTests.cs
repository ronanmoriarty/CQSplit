using System.Collections.Generic;
using System.Linq;
using Cafe.Domain;
using Cafe.Domain.Events;
using NHibernate;
using NUnit.Framework;

namespace CQRSTutorial.DAL.Tests
{
    [TestFixture, Category(TestConstants.Integration)]
    public class EventTests
    {
        private IEventRepository _eventRepository;
        private IEventRepository _eventRepositoryForReadingUncommittedChanges;
        private ISession _writeSession;

        [SetUp]
        public void SetUp()
        {
            _eventRepository = new EventRepository();
            _writeSession = SessionFactory.WriteInstance.OpenSession();
            _writeSession.BeginTransaction();
            _eventRepositoryForReadingUncommittedChanges = new EventRepository();
        }

        [Test]
        public void InsertAndReadTabOpened()
        {
            const string waiter = "John";
            const int tableNumber = 123;

            var tabOpened = new TabOpened
            {
                TableNumber = tableNumber,
                Waiter = waiter
            };

            _eventRepository.Add(tabOpened, _writeSession);
            _writeSession.Flush();

            var retrievedEvent = _eventRepositoryForReadingUncommittedChanges.Read(tabOpened.Id, SessionFactory.ReadInstance.OpenSession());

            Assert.That(retrievedEvent is TabOpened);
            var retrievedTabOpenedEvent = (TabOpened) retrievedEvent;
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

            _eventRepository.Add(foodOrdered, _writeSession);
            _writeSession.Flush();

            var retrievedEvent = _eventRepositoryForReadingUncommittedChanges.Read(foodOrdered.Id, SessionFactory.ReadInstance.OpenSession());

            Assert.That(retrievedEvent is FoodOrdered);
            var retrievedFoodOrderedEvent = (FoodOrdered)retrievedEvent;
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

            _eventRepository.Add(drinkOrdered, _writeSession);
            _writeSession.Flush();

            var readSession = SessionFactory.ReadInstance.OpenSession();
            var retrievedEvent = _eventRepositoryForReadingUncommittedChanges.Read(drinkOrdered.Id, readSession);

            Assert.That(retrievedEvent is DrinksOrdered);
            var retrievedDrinkOrderedEvent = (DrinksOrdered)retrievedEvent;
            Assert.That(retrievedDrinkOrderedEvent.Id, Is.EqualTo(drinkOrdered.Id));
            Assert.That(retrievedDrinkOrderedEvent.Items.Count, Is.EqualTo(1));
            var drinkOrderedItem = retrievedDrinkOrderedEvent.Items.Single();
            Assert.That(drinkOrderedItem.Description, Is.EqualTo(drinkDescription));
            Assert.That(drinkOrderedItem.IsDrink, Is.True);
            Assert.That(drinkOrderedItem.MenuNumber, Is.EqualTo(drinkMenuNumber));
            Assert.That(drinkOrderedItem.Price, Is.EqualTo(drinkPrice));
        }

        [TearDown]
        public void TearDown()
        {
            _writeSession.Transaction?.Rollback();
        }
    }
}
