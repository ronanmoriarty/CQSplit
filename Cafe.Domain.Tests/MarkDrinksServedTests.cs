using System;
using System.Collections.Generic;
using Cafe.Domain.Commands;
using Cafe.Domain.Events;
using Cafe.Waiter.Contracts;
using CQRSTutorial.Core;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Domain.Tests
{
    [TestFixture]
    public class MarkDrinksServedTests : EventTestsBase<Tab, MarkDrinksServed>
    {
        private readonly int _tableNumber = 123;
        private readonly string _waiter = "John Smith";
        private const decimal DrinkPrice = 2m;
        private const int DrinkMenuNumber = 13;
        private const string DrinkDescription = "Coca Cola";
        private const decimal Drink2Price = 2.5m;
        private const int Drink2MenuNumber = 14;
        private const string Drink2Description = "Fanta";

        protected override void ConfigureCommandHandlerFactory(ICommandHandlerFactory commandHandlerFactory)
        {
            commandHandlerFactory.CreateHandlerFor(Arg.Is<MarkDrinksServed>(markDrinksServed => markDrinksServed.AggregateId == AggregateId)).Returns(CommandHandler);
            commandHandlerFactory.CreateHandlerFor(Arg.Is<MarkDrinksServed>(markDrinksServed => markDrinksServed.AggregateId == CommandId2)).Returns(CommandHandler2);
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
                    AggregateId = AggregateId,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new DrinksOrdered
                {
                    AggregateId = AggregateId,
                    Items = new List<OrderedItem>
                    {
                        testDrink1,
                        testDrink2
                    }
                });

            When(new MarkDrinksServed
            {
                Id = CommandId,
                AggregateId = AggregateId,
                MenuNumbers = new List<int>
                {
                    testDrink1.MenuNumber,
                    testDrink2.MenuNumber
                }
            });

            Then(new DrinksServed
            {
                AggregateId = AggregateId,
                CommandId = CommandId,
                MenuNumbers = new List<int>
                { testDrink1.MenuNumber, testDrink2.MenuNumber }
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
                    AggregateId = AggregateId,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new DrinksOrdered
                {
                    AggregateId = AggregateId,
                    Items = new List<OrderedItem> { testDrink1 }
                }
                );

            When(new MarkDrinksServed
            {
                Id = CommandId,
                AggregateId = AggregateId,
                MenuNumbers = new List<int> { testDrink2.MenuNumber }
            });

            Then(new DrinksNotOutstanding
            {
                AggregateId = AggregateId,
                CommandId = CommandId
            });
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
                    AggregateId = AggregateId,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new DrinksOrdered
                {
                    AggregateId = AggregateId,
                    Items = new List<OrderedItem> { testDrink1 }
                },
                new DrinksServed
                {
                    AggregateId = AggregateId,
                    MenuNumbers = new List<int> { testDrink1.MenuNumber }
                }
                );

            When(new MarkDrinksServed
            {
                Id = CommandId,
                AggregateId = AggregateId,
                MenuNumbers = new List<int> { testDrink1.MenuNumber }
            });

            Then(new DrinksNotOutstanding
            {
                AggregateId = AggregateId,
                CommandId = CommandId
            });
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
                    AggregateId = AggregateId,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new DrinksOrdered
                {
                    AggregateId = AggregateId,
                    Items = new List<OrderedItem> { drinkThatWasOrdered }
                }
                );

            var markDrinksServedCommandId = Guid.NewGuid();
            When(new MarkDrinksServed
            {
                Id = markDrinksServedCommandId,
                AggregateId = AggregateId,
                MenuNumbers = new List<int> { drinkThatWasOrdered.MenuNumber, drinkThatWasNotOrdered.MenuNumber }
            });

            Then(new DrinksNotOutstanding
            {
                AggregateId = AggregateId,
                CommandId = markDrinksServedCommandId
            });

            When(new MarkDrinksServed
            {
                Id = CommandId,
                AggregateId = AggregateId,
                MenuNumbers = new List<int> { drinkThatWasOrdered.MenuNumber }
            });

            Then(new DrinksServed
            {
                AggregateId = AggregateId,
                CommandId = CommandId,
                MenuNumbers = new List<int> { drinkThatWasOrdered.MenuNumber }
            });
        }

        protected override Tab GetAggregateToApplyEventsTo()
        {
            return CommandHandler;
        }
    }
}