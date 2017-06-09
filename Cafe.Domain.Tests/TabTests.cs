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

        [Test]
        public void CanOpenANewTab()
        {
            WhenCommandReceived(new OpenTab
            {
                TabId = _tabId,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });

            AssertEventPublished<TabOpened>(AssertTabOpened);
        }

        [Test]
        public void CannotOrderWhenTabHasNotBeenOpenedYet()
        {
            Assert.That(() => WhenCommandReceived(new PlaceOrder { TabId = _tabId, Items = null }), Throws.Exception.InstanceOf<TabNotOpen>());
        }

        [Test]
        public void CanOrderFoodWhenTabHasAlreadyBeenOpened()
        {
            var foodOrderedItem = GetFoodOrderedItem();
            var foodOrderedItems = new List<OrderedItem> { foodOrderedItem };

            WhenCommandReceived(new OpenTab
            {
                TabId = _tabId,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });

            WhenCommandReceived(new PlaceOrder
            {
                TabId = _tabId,
                Items = foodOrderedItems
            });

            AssertEventPublished<FoodOrdered>(x => AssertFoodOrdered(x, foodOrderedItems));
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

        private bool AssertFoodOrdered(FoodOrdered foodOrdered, List<OrderedItem> foodOrderedItems)
        {
            return foodOrdered.Id == _tabId
                   && foodOrdered.Items == foodOrderedItems;
        }

        private bool AssertTabOpened(TabOpened tabOpened)
        {
            return tabOpened.Id == _tabId
                   && tabOpened.TableNumber == _tableNumber
                   && tabOpened.Waiter == _waiter;
        }
    }
}
