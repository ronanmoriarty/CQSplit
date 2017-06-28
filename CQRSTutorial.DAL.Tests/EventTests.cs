using System.Collections.Generic;
using System.Linq;
using Cafe.Domain;
using Cafe.Domain.Events;
using NUnit.Framework;

namespace CQRSTutorial.DAL.Tests
{
    [TestFixture, Category(TestConstants.Integration)]
    public class EventTests
    {
        private IEventRepository _eventRepository;

        [SetUp]
        public void SetUp()
        {
            _eventRepository = new EventRepository(SessionFactory.Instance);
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

            _eventRepository.Add(tabOpened);

            var retrievedEvent = _eventRepository.Read(tabOpened.Id);

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

            _eventRepository.Add(foodOrdered);

            var retrievedEvent = _eventRepository.Read(foodOrdered.Id);

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
    }
}
