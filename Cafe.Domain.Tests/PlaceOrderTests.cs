using System.Collections.Generic;
using Cafe.Domain.Commands;
using Cafe.Domain.Events;
using Cafe.Waiter.Contracts;
using Cafe.Waiter.Contracts.Commands;
using NUnit.Framework;

namespace Cafe.Domain.Tests
{
    [TestFixture]
    public class PlaceOrderTests : EventTestsBase<Tab, PlaceOrder>
    {
        private readonly int _tableNumber = 123;
        private readonly string _waiter = "John Smith";
        private const decimal FoodPrice = 12m;
        private const int FoodMenuNumber = 101;
        private const string FoodDescription = "Tikka Masala";
        private const decimal DrinkPrice = 2m;
        private const int DrinkMenuNumber = 13;
        private const string DrinkDescription = "Coca Cola";

        [Test]
        public void CanOrderFoodWhenTabHasAlreadyBeenOpened()
        {
            var foodOrderedItem = GetFoodOrderedItem();
            var orderedItems = new List<OrderedItem> { foodOrderedItem };

            Given(new TabOpened
            {
                AggregateId = AggregateId,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });

            When(new PlaceOrder
            {
                Id = CommandId,
                AggregateId = AggregateId,
                Items = orderedItems
            });

            Then(new FoodOrdered
            {
                AggregateId = AggregateId,
                CommandId = CommandId,
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
                AggregateId = AggregateId,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });

            When(new PlaceOrder
            {
                Id = CommandId,
                AggregateId = AggregateId,
                Items = orderedItems
            }
                );

            Then(new DrinksOrdered
            {
                AggregateId = AggregateId,
                CommandId = CommandId,
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
                AggregateId = AggregateId,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });

            When(new PlaceOrder
            {
                Id = CommandId,
                AggregateId = AggregateId,
                Items = orderedItems
            }
                );

            Then(
                new DrinksOrdered
                {
                    AggregateId = AggregateId,
                    CommandId = CommandId,
                    Items = new List<OrderedItem> { drinksOrderedItem }
                },
                new FoodOrdered
                {
                    AggregateId = AggregateId,
                    CommandId = CommandId,
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
    }
}