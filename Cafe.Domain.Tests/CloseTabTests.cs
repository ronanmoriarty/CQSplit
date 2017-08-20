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
    public class CloseTabTests : EventTestsBase<Tab>
    {
        private readonly Guid _tabId1 = new Guid("17BEED1C-2084-4ADA-938A-4F850212EB5D");
        private readonly Guid _tabId2 = new Guid("88CEC1FD-A666-4A51-ABD4-3AA49AE35001");
        private readonly int _tableNumber = 123;
        private readonly string _waiter = "John Smith";
        private Tab _tab1;
        private Tab _tab2;
        private Guid _commandId;
        private const decimal DrinkPrice = 2m;
        private const int DrinkMenuNumber = 13;
        private const string DrinkDescription = "Coca Cola";

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
                    AggregateId = _tabId1,
                    TableNumber = _tableNumber,
                    Waiter = _waiter
                },
                new DrinksOrdered
                {
                    AggregateId = _tabId1,
                    Items = new List<OrderedItem> { drinkItem }
                },
                new DrinksServed
                {
                    AggregateId = _tabId1,
                    MenuNumbers = new List<int> { drinkItem.MenuNumber }
                }
                );

            When(new CloseTab
            {
                Id = _commandId,
                AggregateId = _tabId1,
                AmountPaid = drinkItem.Price + 0.50M
            });

            Then(new TabClosed
            {
                AggregateId = _tabId1,
                CommandId = _commandId,
                AmountPaid = drinkItem.Price + 0.50M,
                OrderValue = drinkItem.Price,
                TipValue = 0.50M
            });
        }

        protected override Tab GetSystemUnderTest()
        {
            return _tab1;
        }
    }
}