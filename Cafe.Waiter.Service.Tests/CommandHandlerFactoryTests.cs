using System;
using System.Collections.Generic;
using Cafe.Domain;
using Cafe.Domain.Commands;
using Cafe.Waiter.Contracts;
using CQRSTutorial.Core;
using CQRSTutorial.DAL;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.Service.Tests
{
    [TestFixture]
    public class CommandHandlerFactoryTests
    {
        private readonly Guid _id = new Guid("894AA616-2667-4E61-B205-CEB1BD947277");
        private readonly Guid _aggregateId = new Guid("8E8E2FE0-C223-456C-84ED-A6611AA5F03B");
        private ICommandHandlerFactory _commandHandlerFactory;
        private IEventStore _eventStore;
        private PlaceOrder _placeOrderCommand;
        private Tab _tab;

        [SetUp]
        public void SetUp()
        {
            _eventStore = Substitute.For<IEventStore>();
            _commandHandlerFactory = new CommandHandlerFactory(_eventStore);
            _placeOrderCommand = new PlaceOrder
            {
                Id = _id,
                AggregateId = _aggregateId,
                Items = new List<OrderedItem>()
            };
        }

        [Test]
        public void Retrieves_tab_to_handle_PlaceOrder_command()
        {
            WhenCreatingCommandHandler();

            Assert.That(_tab.Id, Is.EqualTo(_aggregateId));
        }

        [Test]
        public void Retrieved_tab_has_all_previous_events_applied()
        {
            WhenCreatingCommandHandler();

            _eventStore.Received(1).GetAllEventsFor(_aggregateId);
        }

        private void WhenCreatingCommandHandler()
        {
            var commandHandler = _commandHandlerFactory.CreateHandlerFor<IPlaceOrder>(_placeOrderCommand);
            _tab = (Tab) commandHandler;
        }
    }
}
