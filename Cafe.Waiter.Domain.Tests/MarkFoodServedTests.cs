using System;
using System.Collections.Generic;
using Cafe.Waiter.Commands;
using Cafe.Waiter.Contracts.Commands;
using Cafe.Waiter.Domain;
using Cafe.Waiter.Events;
using NUnit.Framework;

namespace Cafe.Domain.Tests
{
    [TestFixture]
    public class MarkFoodServedTests : EventTestsBase<Tab, MarkFoodServedCommand>
    {
        private readonly int _tableNumber = 123;
        private readonly string _waiter = "John Smith";
        private const decimal FoodPrice = 12m;
        private const int FoodMenuNumber = 101;
        private const string FoodDescription = "Tikka Masala";
        private const decimal Food2Price = 15m;
        private const int Food2MenuNumber = 102;
        private const string Food2Description = "Chicken Madras";

        [Test]
        public void OrderedFoodCanBeServed()
        {
            var foodItem1 = new OrderedItem
            {
                Description = FoodDescription,
                IsDrink = false,
                MenuNumber = FoodMenuNumber,
                Price = FoodPrice
            };
            var foodItem2 = new OrderedItem
            {
                Description = Food2Description,
                IsDrink = false,
                MenuNumber = Food2MenuNumber,
                Price = Food2Price
            };

            Given(
                new TabOpened
                {
                    AggregateId = AggregateId,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new FoodOrdered
                {
                    AggregateId = AggregateId,
                    Items = new List<OrderedItem>
                    {
                        foodItem1,
                        foodItem2
                    }
                });

            When(new MarkFoodServedCommand
            {
                Id = CommandId,
                AggregateId = AggregateId,
                MenuNumbers = new List<int>
                {
                    foodItem1.MenuNumber,
                    foodItem2.MenuNumber
                }
            });

            Then(new FoodServed
            {
                AggregateId = AggregateId,
                CommandId = CommandId,
                MenuNumbers = new List<int> { foodItem1.MenuNumber, foodItem2.MenuNumber }
            });
        }

        [Test]
        public void CanNotServeUnorderedFood()
        {
            var orderedFoodItem = new OrderedItem
            {
                Description = FoodDescription,
                IsDrink = false,
                MenuNumber = FoodMenuNumber,
                Price = FoodPrice
            };
            var unorderedFoodItem = new OrderedItem
            {
                Description = Food2Description,
                IsDrink = false,
                MenuNumber = Food2MenuNumber,
                Price = Food2Price
            };

            Given(
                new TabOpened
                {
                    AggregateId = AggregateId,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new FoodOrdered
                {
                    AggregateId = AggregateId,
                    Items = new List<OrderedItem> { orderedFoodItem }
                }
                );

            When(new MarkFoodServedCommand
            {
                Id = CommandId,
                AggregateId = AggregateId,
                MenuNumbers = new List<int> { unorderedFoodItem.MenuNumber }
            });

            Then(new FoodNotOutstanding
            {
                AggregateId = AggregateId,
                CommandId = CommandId
            });
        }

        [Test]
        public void CanNotServeFoodThatHasAlreadyBeenServed()
        {
            var foodItem = new OrderedItem
            {
                Description = FoodDescription,
                IsDrink = false,
                MenuNumber = FoodMenuNumber,
                Price = FoodPrice
            };

            Given(
                new TabOpened
                {
                    AggregateId = AggregateId,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new FoodOrdered
                {
                    AggregateId = AggregateId,
                    Items = new List<OrderedItem> { foodItem }
                },
                new FoodServed
                {
                    AggregateId = AggregateId,
                    MenuNumbers = new List<int> { foodItem.MenuNumber }
                }
                );

            When(new MarkFoodServedCommand
            {
                Id = CommandId,
                AggregateId = AggregateId,
                MenuNumbers = new List<int> { foodItem.MenuNumber }
            });

            Then(new FoodNotOutstanding
            {
                AggregateId = AggregateId,
                CommandId = CommandId
            });
        }

        [Test]
        public void NoFoodMarkedAsServedUnlessAllFoodCanBeMarkedAsServed()
        {
            var foodThatWasOrdered = new OrderedItem
            {
                Description = FoodDescription,
                IsDrink = false,
                MenuNumber = FoodMenuNumber,
                Price = FoodPrice
            };
            var foodThatWasNotOrdered = new OrderedItem
            {
                Description = Food2Description,
                IsDrink = false,
                MenuNumber = Food2MenuNumber,
                Price = Food2Price
            };

            Given(
                new TabOpened
                {
                    AggregateId = AggregateId,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new FoodOrdered
                {
                    AggregateId = AggregateId,
                    Items = new List<OrderedItem> { foodThatWasOrdered }
                }
                );

            var markFoodServedCommandId = Guid.NewGuid();
            When(new MarkFoodServedCommand
            {
                Id = markFoodServedCommandId,
                AggregateId = AggregateId,
                MenuNumbers = new List<int> { foodThatWasOrdered.MenuNumber, foodThatWasNotOrdered.MenuNumber }
            });

            Then(new FoodNotOutstanding
            {
                AggregateId = AggregateId,
                CommandId = markFoodServedCommandId
            });

            When(new MarkFoodServedCommand
            {
                Id = CommandId,
                AggregateId = AggregateId,
                MenuNumbers = new List<int> { foodThatWasOrdered.MenuNumber }
            });

            Then(new FoodServed
            {
                AggregateId = AggregateId,
                CommandId = CommandId,
                MenuNumbers = new List<int> { foodThatWasOrdered.MenuNumber }
            });
        }
    }
}