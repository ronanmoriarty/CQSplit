using System;
using System.Collections.Generic;
using Cafe.Domain;
using Cafe.Domain.Commands;
using Cafe.Waiter.Contracts;
using CQRSTutorial.Core;
using NUnit.Framework;

namespace Cafe.Waiter.Service.Tests
{
    [TestFixture]
    public class CommandHandlerFactoryTests
    {
        private readonly Guid _id = new Guid("894AA616-2667-4E61-B205-CEB1BD947277");
        private readonly Guid _aggregateId = new Guid("8E8E2FE0-C223-456C-84ED-A6611AA5F03B");
        private ICommandHandlerFactory _commandHandlerFactory;

        [SetUp]
        public void SetUp()
        {
            _commandHandlerFactory = new CommandHandlerFactory();
        }

        [Test]
        public void Retrieves_tab_to_handle_PlaceOrder_command()
        {
            var placeOrder = new PlaceOrder()
            {
                Id = _id,
                AggregateId = _aggregateId,
                Items = new List<OrderedItem>()
            };

            var commandHandler = _commandHandlerFactory.CreateHandlerFor<IPlaceOrder>(placeOrder);

            Assert.That(commandHandler, Is.TypeOf<Tab>());
            var tab = (Tab)commandHandler;
            Assert.That(tab.Id, Is.EqualTo(_aggregateId));
        }
    }
}
