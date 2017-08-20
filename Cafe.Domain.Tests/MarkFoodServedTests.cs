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
    public class MarkFoodServedTests : EventTestsBase<Tab, MarkFoodServed>
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
        private const decimal Food2Price = 15m;
        private const int Food2MenuNumber = 102;
        private const string Food2Description = "Chicken Madras";

        protected override IAggregateStore GetAggregateStore()
        {
            ReinitialiseForNextTest();
            return new FakeAggregateStore(new List<ICommandHandler> { _tab1, _tab2, new TabFactory() });
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
                    AggregateId = _tabId1,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new FoodOrdered
                {
                    AggregateId = _tabId1,
                    Items = new List<OrderedItem>
                    {
                        foodItem1,
                        foodItem2
                    }
                });

            When(new MarkFoodServed
            {
                Id = _commandId,
                AggregateId = _tabId1,
                MenuNumbers = new List<int>
                {
                    foodItem1.MenuNumber,
                    foodItem2.MenuNumber
                }
            });

            Then(new FoodServed
            {
                AggregateId = _tabId1,
                CommandId = _commandId,
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
                    AggregateId = _tabId1,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new FoodOrdered
                {
                    AggregateId = _tabId1,
                    Items = new List<OrderedItem> { orderedFoodItem }
                }
                );

            When(new MarkFoodServed
            {
                Id = _commandId,
                AggregateId = _tabId1,
                MenuNumbers = new List<int> { unorderedFoodItem.MenuNumber }
            });

            Then(new FoodNotOutstanding
            {
                AggregateId = _tabId1,
                CommandId = _commandId
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
                    AggregateId = _tabId1,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new FoodOrdered
                {
                    AggregateId = _tabId1,
                    Items = new List<OrderedItem> { foodItem }
                },
                new FoodServed
                {
                    AggregateId = _tabId1,
                    MenuNumbers = new List<int> { foodItem.MenuNumber }
                }
                );

            When(new MarkFoodServed
            {
                Id = _commandId,
                AggregateId = _tabId1,
                MenuNumbers = new List<int> { foodItem.MenuNumber }
            });

            Then(new FoodNotOutstanding
            {
                AggregateId = _tabId1,
                CommandId = _commandId
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
                    AggregateId = _tabId1,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new FoodOrdered
                {
                    AggregateId = _tabId1,
                    Items = new List<OrderedItem> { foodThatWasOrdered }
                }
                );

            var markFoodServedCommandId = Guid.NewGuid();
            When(new MarkFoodServed
            {
                Id = markFoodServedCommandId,
                AggregateId = _tabId1,
                MenuNumbers = new List<int> { foodThatWasOrdered.MenuNumber, foodThatWasNotOrdered.MenuNumber }
            });

            Then(new FoodNotOutstanding
            {
                AggregateId = _tabId1,
                CommandId = markFoodServedCommandId
            });

            When(new MarkFoodServed
            {
                Id = _commandId,
                AggregateId = _tabId1,
                MenuNumbers = new List<int> { foodThatWasOrdered.MenuNumber }
            });

            Then(new FoodServed
            {
                AggregateId = _tabId1,
                CommandId = _commandId,
                MenuNumbers = new List<int> { foodThatWasOrdered.MenuNumber }
            });
        }

        protected override Tab GetSystemUnderTest()
        {
            return _tab1;
        }
    }
}