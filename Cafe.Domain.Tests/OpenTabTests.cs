﻿using System;
using Cafe.Domain.Commands;
using Cafe.Domain.Events;
using CQRSTutorial.Core;
using NUnit.Framework;

namespace Cafe.Domain.Tests
{
    [TestFixture]
    public class OpenTabTests : EventTestsBase<Tab, OpenTab>
    {
        private readonly Guid _tabId1 = new Guid("17BEED1C-2084-4ADA-938A-4F850212EB5D");
        private readonly int _tableNumber = 123;
        private readonly string _waiter = "John Smith";
        private readonly Guid _commandId = new Guid("EEB9AF2B-0399-44D3-87D0-DBEC8699614E");

        protected override void ConfigureCommandHandlerFactory(ICommandHandlerFactory commandHandlerFactory)
        {
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
    }
}