using System;
using System.Collections.Generic;
using Cafe.Domain.Commands;
using Cafe.Domain.Events;
using Cafe.Waiter.Contracts;
using CQRSTutorial.Core;
using CQRSTutorial.Tests.Common;
using NUnit.Framework;

namespace Cafe.Domain.Tests
{
    [TestFixture]
    public class PlaceOrderTests : EventTestsBase<Tab, PlaceOrder>
    {
        private readonly Guid _tabId1 = new Guid("17BEED1C-2084-4ADA-938A-4F850212EB5D");
        private readonly Guid _tabId2 = new Guid("88CEC1FD-A666-4A51-ABD4-3AA49AE35001");
        private readonly int _tableNumber = 123;
        private readonly string _waiter = "John Smith";
        private Tab _tab1;
        private Tab _tab2;
        private Guid _commandId;
        private const decimal FoodPrice = 12m;
        private const int FoodMenuNumber = 101;
        private const string FoodDescription = "Tikka Masala";
        private const decimal DrinkPrice = 2m;
        private const int DrinkMenuNumber = 13;
        private const string DrinkDescription = "Coca Cola";

        protected override ICommandHandlerProvider GetCommandHandlerProvider()
        {
            ReinitialiseForNextTest();
            var fakeCommandHandlerProvider = new FakeCommandHandlerProvider();
            fakeCommandHandlerProvider.RegisterCommandHandler(_tab1);
            fakeCommandHandlerProvider.RegisterCommandHandler(_tab2);
            return fakeCommandHandlerProvider;
        }

        private void ReinitialiseForNextTest()
        {
            _commandId = Guid.NewGuid();
            _tab1 = new Tab
            {
                Id = _tabId1
            };
            _tab2 = new Tab
            {
                Id = _tabId2
            };
        }

        [Test]
        public void CanOrderFoodWhenTabHasAlreadyBeenOpened()
        {
            var foodOrderedItem = GetFoodOrderedItem();
            var orderedItems = new List<OrderedItem> { foodOrderedItem };

            Given(new TabOpened
            {
                AggregateId = _tabId1,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });

            When(new PlaceOrder
            {
                Id = _commandId,
                AggregateId = _tabId1,
                Items = orderedItems
            });

            Then(new FoodOrdered
            {
                AggregateId = _tabId1,
                CommandId = _commandId,
                Items = orderedItems
            });
        }

        [Test]
        public void CanOrderDrinksWhenTabHasAlreadyBeenOpened()
        {
            var drinksOrderedItem = GetDrinkOrderedItem();
            var orderedItems = new List<OrderedItem> { drinksOrderedItem };

            Given(new TabOpened
            {
                AggregateId = _tabId1,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });

            When(new PlaceOrder
            {
                Id = _commandId,
                AggregateId = _tabId1,
                Items = orderedItems
            }
                );

            Then(new DrinksOrdered
            {
                AggregateId = _tabId1,
                CommandId = _commandId,
                Items = orderedItems
            });
        }

        [Test]
        public void CanOrderFoodAndDrinksWhenTabHasAlreadyBeenOpened()
        {
            var foodOrderedItem = GetFoodOrderedItem();
            var drinksOrderedItem = GetDrinkOrderedItem();
            var orderedItems = new List<OrderedItem> { foodOrderedItem, drinksOrderedItem };

            Given(new TabOpened
            {
                AggregateId = _tabId1,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });

            When(new PlaceOrder
            {
                Id = _commandId,
                AggregateId = _tabId1,
                Items = orderedItems
            }
                );

            Then(
                new DrinksOrdered
                {
                    AggregateId = _tabId1,
                    CommandId = _commandId,
                    Items = new List<OrderedItem> { drinksOrderedItem }
                },
                new FoodOrdered
                {
                    AggregateId = _tabId1,
                    CommandId = _commandId,
                    Items = new List<OrderedItem> { foodOrderedItem }
                }
                );
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

        protected override Tab GetAggregateToApplyEventsTo()
        {
            return _tab1;
        }
    }
}