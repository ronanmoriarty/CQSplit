using System.Collections.Generic;
using Cafe.Domain.Commands;
using Cafe.Domain.Events;
using Cafe.Domain.Exceptions;
using CQRSTutorial.Core;
using CQRSTutorial.Tests.Common;
using NUnit.Framework;

namespace Cafe.Domain.Tests
{
    [TestFixture]
    public class TabTests : EventTestsBase<Tab>
    {
        private const int TabId1 = 321;
        private int TabId2 = 234;
        private readonly int _tableNumber = 123;
        private readonly string _waiter = "John Smith";
        private Tab _tab1;
        private Tab _tab2;
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

        protected override IAggregateStore GetAggregateStore()
        {
            ReinitialiseTabs();
            return new AggregateStore(new List<ICommandHandler> { _tab1, _tab2, new FakeTabFactory(TabId1) });
        }

        private void ReinitialiseTabs()
        {
            _tab1 = new Tab
            {
                Id = TabId1
            };
            _tab2 = new Tab
            {
                Id = TabId2
            };
        }

        [Test]
        public void CanOpenANewTab()
        {
            When(new OpenTab
            {
                TableNumber = _tableNumber,
                Waiter = _waiter
            });

            Then(new TabOpened
            {
                AggregateId = TabId1,
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
                AggregateId = TabId1,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });

            When(new PlaceOrder
            {
                AggregateId = TabId1,
                Items = orderedItems
            });

            Then(new FoodOrdered
            {
                AggregateId = TabId1,
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
                AggregateId = TabId1,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });

            When(new PlaceOrder
            {
                AggregateId = TabId1,
                Items = orderedItems
            }
            );

            Then(new DrinksOrdered
            {
                AggregateId = TabId1,
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
                AggregateId = TabId1,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });

            When(new PlaceOrder
            {
                AggregateId = TabId1,
                Items = orderedItems
            }
            );

            Then(
                new DrinksOrdered
                {
                    AggregateId = TabId1,
                    Items = new List<OrderedItem> { drinksOrderedItem }
                },
                new FoodOrdered
                {
                    AggregateId = TabId1,
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
                    AggregateId = TabId1,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new DrinksOrdered
                {
                    AggregateId = TabId1,
                    Items = new List<OrderedItem>
                    {
                        testDrink1,
                        testDrink2
                    }
                });

            When(new MarkDrinksServed
            {
                AggregateId = TabId1,
                MenuNumbers = new List<int>
                {
                    testDrink1.MenuNumber,
                    testDrink2.MenuNumber
                }
            });

            Then(new DrinksServed
            {
                AggregateId = TabId1,
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
                    AggregateId = TabId1,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new FoodOrdered
                {
                    AggregateId = TabId1,
                    Items = new List<OrderedItem>
                    {
                        foodItem1,
                        foodItem2
                    }
                });

            When(new MarkFoodServed
            {
                AggregateId = TabId1,
                MenuNumbers = new List<int>
                {
                    foodItem1.MenuNumber,
                    foodItem2.MenuNumber
                }
            });

            Then(new FoodServed
            {
                AggregateId = TabId1,
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
                    AggregateId = TabId1,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new DrinksOrdered
                {
                    AggregateId = TabId1,
                    Items = new List<OrderedItem> { testDrink1 }
                }
            );

            When(new MarkDrinksServed
            {
                AggregateId = TabId1,
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
                    AggregateId = TabId1,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new FoodOrdered
                {
                    AggregateId = TabId1,
                    Items = new List<OrderedItem> { orderedFoodItem }
                }
            );

            When(new MarkFoodServed
            {
                AggregateId = TabId1,
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
                    AggregateId = TabId1,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new DrinksOrdered
                {
                    AggregateId = TabId1,
                    Items = new List<OrderedItem> { testDrink1 }
                },
                new DrinksServed
                {
                    AggregateId = TabId1,
                    MenuNumbers = new List<int> { testDrink1.MenuNumber }
                }
            );

            When(new MarkDrinksServed
            {
                AggregateId = TabId1,
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
                    AggregateId = TabId1,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new FoodOrdered
                {
                    AggregateId = TabId1,
                    Items = new List<OrderedItem> { foodItem }
                },
                new FoodServed
                {
                    AggregateId = TabId1,
                    MenuNumbers = new List<int> { foodItem.MenuNumber }
                }
            );

            When(new MarkFoodServed
            {
                AggregateId = TabId1,
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
                    AggregateId = TabId1,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new DrinksOrdered
                {
                    AggregateId = TabId1,
                    Items = new List<OrderedItem> { drinkThatWasOrdered }
                }
            );

            When(new MarkDrinksServed
            {
                AggregateId = TabId1,
                MenuNumbers = new List<int> { drinkThatWasOrdered.MenuNumber, drinkThatWasNotOrdered.MenuNumber }
            });

            Then(new DrinksNotOutstanding());

            When(new MarkDrinksServed
            {
                AggregateId = TabId1,
                MenuNumbers = new List<int> { drinkThatWasOrdered.MenuNumber }
            });

            Then(new DrinksServed
            {
                AggregateId = TabId1,
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
                    AggregateId = TabId1,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new FoodOrdered
                {
                    AggregateId = TabId1,
                    Items = new List<OrderedItem> { foodThatWasOrdered }
                }
            );

            When(new MarkFoodServed
            {
                AggregateId = TabId1,
                MenuNumbers = new List<int> { foodThatWasOrdered.MenuNumber, foodThatWasNotOrdered.MenuNumber }
            });

            Then(new FoodNotOutstanding());

            When(new MarkFoodServed
            {
                AggregateId = TabId1,
                MenuNumbers = new List<int> { foodThatWasOrdered.MenuNumber }
            });

            Then(new FoodServed
            {
                AggregateId = TabId1,
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
                    AggregateId = TabId1,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new DrinksOrdered
                {
                    AggregateId = TabId1,
                    Items = new List<OrderedItem> { drinkItem }
                },
                new DrinksServed
                {
                    AggregateId = TabId1,
                    MenuNumbers = new List<int> { drinkItem.MenuNumber }
                }
            );

            When(new CloseTab
            {
                AggregateId = TabId1,
                AmountPaid = drinkItem.Price + 0.50M
            });

            Then(new TabClosed
            {
                AggregateId = TabId1,
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

        protected override Tab GetSystemUnderTest()
        {
            return _tab1;
        }
    }

    public class FakeTabFactory : ICommandHandler<OpenTab>
    {
        private readonly int _tabId;

        public FakeTabFactory(int tabId)
        {
            _tabId = tabId;
        }

        public IEnumerable<IEvent> Handle(OpenTab command)
        {
            return new IEvent[]
            {
                new TabOpened
                {
                    AggregateId = _tabId,
                    TableNumber = command.TableNumber,
                    Waiter = command.Waiter
                }
            };
        }

        public bool CanHandle(ICommand command)
        {
            return command.GetType() == typeof(OpenTab);
        }
    }
}