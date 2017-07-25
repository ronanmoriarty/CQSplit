using System.Collections.Generic;
using Cafe.Domain.Commands;
using Cafe.Domain.Events;
using Cafe.Domain.Exceptions;
using CQRSTutorial.Tests.Common;
using NUnit.Framework;

namespace Cafe.Domain.Tests
{
    [TestFixture]
    public class TabTests : EventTestsBase<Tab>
    {
        private readonly int _tabId = 321;
        private readonly int _tableNumber = 123;
        private readonly string _waiter = "John Smith";
        private const decimal FoodPrice = 12m;
        private const int FoodMenuNumber = 101;
        private const string FoodDescription = "Tikka Masala";
        private const decimal Food2Price = 15m;
        private const int Food2MenuNumber = 102;
        private const string Food2Description = "Chicken Madras";
        private const decimal DrinkPrice = 2m;
        private const int DrinkMenuNumber = 13;
        private const string DrinkDescription = "Coca Cola";
        private const decimal Drink2Price = 2.5m;
        private const int Drink2MenuNumber = 14;
        private const string Drink2Description = "Fanta";

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
                AggregateId = _tabId,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });
        }

        [Test]
        public void CanOrderFoodWhenTabHasAlreadyBeenOpened()
        {
            var foodOrderedItem = GetFoodOrderedItem();
            var orderedItems = new List<OrderedItem> { foodOrderedItem };

            Given(new TabOpened
            {
                AggregateId = _tabId,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });

            When(new PlaceOrder
            {
                TabId = _tabId,
                Items = orderedItems
            });

            Then(new FoodOrdered
            {
                AggregateId = _tabId,
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
                AggregateId = _tabId,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });

            When(new PlaceOrder
            {
                TabId = _tabId,
                Items = orderedItems
            }
            );

            Then(new DrinksOrdered
            {
                AggregateId = _tabId,
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
                AggregateId = _tabId,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });

            When(new PlaceOrder
            {
                TabId = _tabId,
                Items = orderedItems
            }
            );

            Then(
                new DrinksOrdered
                {
                    AggregateId = _tabId,
                    Items = new List<OrderedItem> { drinksOrderedItem }
                },
                new FoodOrdered
                {
                    AggregateId = _tabId,
                    Items = new List<OrderedItem> { foodOrderedItem }
                }
            );
        }

        [Test]
        public void OrderedDrinksCanBeServed()
        {
            var testDrink1 = new OrderedItem
            {
                Description = DrinkDescription,
                IsDrink = true,
                MenuNumber = DrinkMenuNumber,
                Price = DrinkPrice
            };
            var testDrink2 = new OrderedItem
            {
                Description = Drink2Description,
                IsDrink = true,
                MenuNumber = Drink2MenuNumber,
                Price = Drink2Price
            };

            Given(
                new TabOpened
                {
                    AggregateId = _tabId,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new DrinksOrdered
                {
                    AggregateId = _tabId,
                    Items = new List<OrderedItem>
                    {
                        testDrink1,
                        testDrink2
                    }
                });

            When(new MarkDrinksServed
            {
                TabId = _tabId,
                MenuNumbers = new List<int>
                {
                    testDrink1.MenuNumber,
                    testDrink2.MenuNumber
                }
            });

            Then(new DrinksServed
            {
                AggregateId = _tabId,
                MenuNumbers = new List<int>
                        { testDrink1.MenuNumber, testDrink2.MenuNumber }
            });
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
                    AggregateId = _tabId,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new FoodOrdered
                {
                    AggregateId = _tabId,
                    Items = new List<OrderedItem>
                    {
                        foodItem1,
                        foodItem2
                    }
                });

            When(new MarkFoodServed
            {
                TabId = _tabId,
                MenuNumbers = new List<int>
                {
                    foodItem1.MenuNumber,
                    foodItem2.MenuNumber
                }
            });

            Then(new FoodServed
            {
                AggregateId = _tabId,
                MenuNumbers = new List<int> { foodItem1.MenuNumber, foodItem2.MenuNumber }
            });
        }

        [Test]
        public void CanNotServeAnUnorderedDrink()
        {
            var testDrink1 = new OrderedItem
            {
                Description = DrinkDescription,
                IsDrink = true,
                MenuNumber = DrinkMenuNumber,
                Price = DrinkPrice
            };
            var testDrink2 = new OrderedItem
            {
                Description = Drink2Description,
                IsDrink = true,
                MenuNumber = Drink2MenuNumber,
                Price = Drink2Price
            };

            Given(
                new TabOpened
                {
                    AggregateId = _tabId,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new DrinksOrdered
                {
                    AggregateId = _tabId,
                    Items = new List<OrderedItem> { testDrink1 }
                }
            );

            When(new MarkDrinksServed
            {
                TabId = _tabId,
                MenuNumbers = new List<int> { testDrink2.MenuNumber }
            });

            Then(new DrinksNotOutstanding());
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
                    AggregateId = _tabId,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new FoodOrdered
                {
                    AggregateId = _tabId,
                    Items = new List<OrderedItem> { orderedFoodItem }
                }
            );

            When(new MarkFoodServed
            {
                TabId = _tabId,
                MenuNumbers = new List<int> { unorderedFoodItem.MenuNumber }
            });

            Then(new FoodNotOutstanding());
        }

        [Test]
        public void CanNotServeADrinkThatHasAlreadyBeenServed()
        {
            var testDrink1 = new OrderedItem
            {
                Description = DrinkDescription,
                IsDrink = true,
                MenuNumber = DrinkMenuNumber,
                Price = DrinkPrice
            };

            Given(
                new TabOpened
                {
                    AggregateId = _tabId,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new DrinksOrdered
                {
                    AggregateId = _tabId,
                    Items = new List<OrderedItem> { testDrink1 }
                },
                new DrinksServed
                {
                    AggregateId = _tabId,
                    MenuNumbers = new List<int> { testDrink1.MenuNumber }
                }
            );

            When(new MarkDrinksServed
            {
                TabId = _tabId,
                MenuNumbers = new List<int> { testDrink1.MenuNumber }
            });

            Then(new DrinksNotOutstanding());
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
                    AggregateId = _tabId,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new FoodOrdered
                {
                    AggregateId = _tabId,
                    Items = new List<OrderedItem> { foodItem }
                },
                new FoodServed
                {
                    AggregateId = _tabId,
                    MenuNumbers = new List<int> { foodItem.MenuNumber }
                }
            );

            When(new MarkFoodServed
            {
                TabId = _tabId,
                MenuNumbers = new List<int> { foodItem.MenuNumber }
            });

            Then(new FoodNotOutstanding());
        }

        [Test]
        public void NoDrinksMarkedAsServedUnlessAllDrinksCanBeMarkedAsServed()
        {
            var drinkThatWasOrdered = new OrderedItem
            {
                Description = DrinkDescription,
                IsDrink = true,
                MenuNumber = DrinkMenuNumber,
                Price = DrinkPrice
            };
            var drinkThatWasNotOrdered = new OrderedItem
            {
                Description = Drink2Description,
                IsDrink = true,
                MenuNumber = Drink2MenuNumber,
                Price = Drink2Price
            };

            Given(
                new TabOpened
                {
                    AggregateId = _tabId,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new DrinksOrdered
                {
                    AggregateId = _tabId,
                    Items = new List<OrderedItem> { drinkThatWasOrdered }
                }
            );

            When(new MarkDrinksServed
            {
                TabId = _tabId,
                MenuNumbers = new List<int> { drinkThatWasOrdered.MenuNumber, drinkThatWasNotOrdered.MenuNumber }
            });

            Then(new DrinksNotOutstanding());

            When(new MarkDrinksServed
            {
                TabId = _tabId,
                MenuNumbers = new List<int> { drinkThatWasOrdered.MenuNumber }
            });

            Then(new DrinksServed
            {
                AggregateId = _tabId,
                MenuNumbers = new List<int> { drinkThatWasOrdered.MenuNumber }
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
                    AggregateId = _tabId,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new FoodOrdered
                {
                    AggregateId = _tabId,
                    Items = new List<OrderedItem> { foodThatWasOrdered }
                }
            );

            When(new MarkFoodServed
            {
                TabId = _tabId,
                MenuNumbers = new List<int> { foodThatWasOrdered.MenuNumber, foodThatWasNotOrdered.MenuNumber }
            });

            Then(new FoodNotOutstanding());

            When(new MarkFoodServed
            {
                TabId = _tabId,
                MenuNumbers = new List<int> { foodThatWasOrdered.MenuNumber }
            });

            Then(new FoodServed
            {
                AggregateId = _tabId,
                MenuNumbers = new List<int> { foodThatWasOrdered.MenuNumber }
            });
        }

        [Test]
        public void CanCloseTabWithTip()
        {
            var drinkItem = new OrderedItem
            {
                Description = DrinkDescription,
                IsDrink = true,
                MenuNumber = DrinkMenuNumber,
                Price = DrinkPrice
            };

            Given(
                new TabOpened
                {
                    AggregateId = _tabId,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new DrinksOrdered
                {
                    AggregateId = _tabId,
                    Items = new List<OrderedItem> {drinkItem}
                },
                new DrinksServed
                {
                    AggregateId = _tabId,
                    MenuNumbers = new List<int> {drinkItem.MenuNumber}
                }
            );

            When(new CloseTab
            {
                TabId = _tabId,
                AmountPaid = drinkItem.Price + 0.50M
            });

            Then(new TabClosed
            {
                AggregateId = _tabId,
                AmountPaid = drinkItem.Price + 0.50M,
                OrderValue = drinkItem.Price,
                TipValue = 0.50M
            });
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

        protected override object[] GetCommandHandlers()
        {
            return new object[] {new Tab(), new TabFactory()};
        }
    }
}