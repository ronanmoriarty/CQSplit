using System;
using System.Collections.Generic;
using Cafe.Domain.Commands;
using Cafe.Domain.Events;
using Cafe.Domain.Exceptions;
using NUnit.Framework;

namespace Cafe.Domain.Tests
{
    [TestFixture]
    public class TabTests : EventTestsBase<Tab>
    {
        private readonly Guid _tabId = new Guid("91EBA94D-3A5F-45FD-BEC4-712E631C732C");
        private readonly int _tableNumber = 123;
        private readonly string _waiter = "John Smith";
        private const decimal FoodPrice = 12m;
        private const int FoodMenuNumber = 12;
        private const string FoodDescription = "Tikka Masala";
        private const decimal DrinkPrice = 2m;
        private const int DrinkMenuNumber = 13;
        private const string DrinkDescription = "Coca Cola";

        [Test]
        public void CanOpenANewTab()
        {
            When(new OpenTab
            {
                TabId = _tabId,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });

            Then(new TabOpened
            {
                Id = _tabId,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });
        }

        [Test]
        public void CannotOrderWhenTabHasNotBeenOpenedYet()
        {
            Assert.That(() => When(new PlaceOrder { TabId = _tabId, Items = null }), Throws.Exception.InstanceOf<TabNotOpen>());
        }

        [Test]
        public void CanOrderFoodWhenTabHasAlreadyBeenOpened()
        {
            var foodOrderedItem = GetFoodOrderedItem();
            var orderedItems = new List<OrderedItem> { foodOrderedItem };

            When(new OpenTab
            {
                TabId = _tabId,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });
            When(new PlaceOrder
            {
                TabId = _tabId,
                Items = orderedItems
            });

            AssertEventPublished<FoodOrdered>(x => AssertFoodOrdered(x, orderedItems));
        }

        [Test]
        public void CanOrderDrinksWhenTabHasAlreadyBeenOpened()
        {
            var drinksOrderedItem = GetDrinkOrderedItem();
            var orderedItems = new List<OrderedItem> { drinksOrderedItem };

            When(new OpenTab
            {
                TabId = _tabId,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });
            When(new PlaceOrder
            {
                TabId = _tabId,
                Items = orderedItems
            });

            AssertEventPublished<DrinksOrdered>(x => AssertDrinksOrdered(x, orderedItems));
        }

        [Test]
        public void CanOrderFoodAndDrinksWhenTabHasAlreadyBeenOpened()
        {
            var foodOrderedItem = GetFoodOrderedItem();
            var drinksOrderedItem = GetDrinkOrderedItem();
            var orderedItems = new List<OrderedItem> {foodOrderedItem, drinksOrderedItem};

            When(new OpenTab
            {
                TabId = _tabId,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });
            When(new PlaceOrder
            {
                TabId = _tabId,
                Items = orderedItems
            });

            AssertEventPublished<DrinksOrdered>(x => AssertDrinksOrdered(x, new List<OrderedItem> { drinksOrderedItem }));
            AssertEventPublished<FoodOrdered>(x => AssertFoodOrdered(x, new List<OrderedItem> { foodOrderedItem }));
        }

        private OrderedItem GetFoodOrderedItem()
        {
            return new OrderedItem
            {
                Description = FoodDescription,
                IsDrink = false,
                MenuNumber = FoodMenuNumber,
                Price = FoodPrice
            };
        }

        private OrderedItem GetDrinkOrderedItem()
        {
            return new OrderedItem
            {
                Description = DrinkDescription,
                IsDrink = true,
                MenuNumber = DrinkMenuNumber,
                Price = DrinkPrice
            };
        }

        private bool AssertFoodOrdered(FoodOrdered foodOrdered, List<OrderedItem> orderedItems)
        {
            Assert.That(foodOrdered.Id, Is.EqualTo(_tabId));
            CollectionAssert.AreEquivalent(foodOrdered.Items, orderedItems);
            return true;
        }

        private bool AssertDrinksOrdered(DrinksOrdered drinksOrdered, List<OrderedItem> orderedItems)
        {
            Assert.That(drinksOrdered.Id, Is.EqualTo(_tabId));
            CollectionAssert.AreEquivalent(drinksOrdered.Items, orderedItems);
            return true;
        }

        private bool AssertTabOpened(TabOpened tabOpened)
        {
            return tabOpened.Id == _tabId
                   && tabOpened.TableNumber == _tableNumber
                   && tabOpened.Waiter == _waiter;
        }
    }
}
