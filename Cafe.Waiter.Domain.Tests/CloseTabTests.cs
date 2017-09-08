using System.Collections.Generic;
using Cafe.Waiter.Contracts;
using Cafe.Waiter.Contracts.Commands;
using Cafe.Waiter.Domain;
using Cafe.Waiter.Domain.Commands;
using Cafe.Waiter.Domain.Events;
using NUnit.Framework;

namespace Cafe.Domain.Tests
{
    [TestFixture]
    public class CloseTabTests : EventTestsBase<Tab, CloseTabCommand>
    {
        private readonly int _tableNumber = 123;
        private readonly string _waiter = "John Smith";
        private const decimal DrinkPrice = 2m;
        private const int DrinkMenuNumber = 13;
        private const string DrinkDescription = "Coca Cola";

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
                    AggregateId = AggregateId,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new DrinksOrdered
                {
                    AggregateId = AggregateId,
                    Items = new List<OrderedItem> { drinkItem }
                },
                new DrinksServed
                {
                    AggregateId = AggregateId,
                    MenuNumbers = new List<int> { drinkItem.MenuNumber }
                }
                );

            When(new CloseTabCommand
            {
                Id = CommandId,
                AggregateId = AggregateId,
                AmountPaid = drinkItem.Price + 0.50M
            });

            Then(new TabClosed
            {
                AggregateId = AggregateId,
                CommandId = CommandId,
                AmountPaid = drinkItem.Price + 0.50M,
                OrderValue = drinkItem.Price,
                TipValue = 0.50M
            });
        }
    }
}