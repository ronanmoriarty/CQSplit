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
    public class CloseTabTests : EventTestsBase<Tab, CloseTab>
    {
        private readonly Guid _tabId2 = new Guid("88CEC1FD-A666-4A51-ABD4-3AA49AE35001");
        private readonly int _tableNumber = 123;
        private readonly string _waiter = "John Smith";
        private Tab _tab1;
        private Tab _tab2;
        private Guid _commandId;
        private const decimal DrinkPrice = 2m;
        private const int DrinkMenuNumber = 13;
        private const string DrinkDescription = "Coca Cola";

        protected override void ConfigureCommandHandlerFactory(ICommandHandlerFactory commandHandlerFactory)
        {
            ReinitialiseForNextTest();
            commandHandlerFactory.CreateHandlerFor(Arg.Is<CloseTab>(closeTab => closeTab.AggregateId == AggregateId)).Returns(_tab1);
            commandHandlerFactory.CreateHandlerFor(Arg.Is<CloseTab>(closeTab => closeTab.AggregateId == _tabId2)).Returns(_tab2);
        }

        private void ReinitialiseForNextTest()
        {
            _commandId = Guid.NewGuid();
            _tab1 = new Tab
            {
                Id = AggregateId
            };
            _tab2 = new Tab
            {
                Id = _tabId2
            };
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

            When(new CloseTab
            {
                Id = _commandId,
                AggregateId = AggregateId,
                AmountPaid = drinkItem.Price + 0.50M
            });

            Then(new TabClosed
            {
                AggregateId = AggregateId,
                CommandId = _commandId,
                AmountPaid = drinkItem.Price + 0.50M,
                OrderValue = drinkItem.Price,
                TipValue = 0.50M
            });
        }

        protected override Tab GetAggregateToApplyEventsTo()
        {
            return _tab1;
        }
    }
}