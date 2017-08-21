using System;
using System.Collections.Generic;
using Cafe.Domain.Commands;
using Cafe.Domain.Events;
using CQRSTutorial.Core;
using CQRSTutorial.Tests.Common;
using NUnit.Framework;

namespace Cafe.Domain.Tests
{
    [TestFixture]
    public class OpenTabTests : EventTestsBase<Tab, OpenTab>
    {
        private readonly Guid _tabId1 = new Guid("17BEED1C-2084-4ADA-938A-4F850212EB5D");
        private readonly int _tableNumber = 123;
        private readonly string _waiter = "John Smith";
        private Tab _tab1;
        private Guid _commandId;

        protected override ICommandHandlerProvider GetCommandHandlerProvider()
        {
            _commandId = Guid.NewGuid();
            _tab1 = new Tab
            {
                Id = _tabId1
            };
            return new FakeCommandHandlerProvider(new List<ICommandHandler> { _tab1, new TabFactory() });
        }

        [Test]
        public void CanOpenANewTab()
        {
            When(new OpenTab
            {
                Id = _commandId,
                AggregateId = _tabId1,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });

            Then(new TabOpened
            {
                AggregateId = _tabId1,
                CommandId = _commandId,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });
        }

        protected override Tab GetAggregateToApplyEventsTo()
        {
            return _tab1;
        }
    }
}