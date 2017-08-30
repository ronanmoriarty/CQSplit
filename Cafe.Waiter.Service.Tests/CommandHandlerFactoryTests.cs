using System;
using System.Collections.Generic;
using Cafe.Domain;
using Cafe.Domain.Commands;
using Cafe.Domain.Events;
using Cafe.Waiter.Contracts.Commands;
using CQRSTutorial.Core;
using CQRSTutorial.DAL;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.Service.Tests
{
    [TestFixture]
    public class CommandHandlerFactoryTests
    {
        private readonly Guid _markDrinksServedCommandId = new Guid("894AA616-2667-4E61-B205-CEB1BD947277");
        private readonly Guid _tabOpenedEventId = new Guid("CF0C9A4B-B389-4D6A-802D-6C7E16D222CA");
        private readonly Guid _openTabCommandId = new Guid("7F76EB72-0AE4-4428-8B96-0544A0D9D10D");
        private readonly int _tableNumber = 123;
        private readonly string _waiter = "John";
        private readonly Guid _placeOrderCommandId = new Guid("92D2AE15-E961-4372-BB61-184642F0302F");
        private DrinksOrdered _drinksOrdered;
        private readonly Guid _drinksOrderedId = new Guid("91FB4A70-F11A-4D57-824D-DD2A224EBAB2");
        private string _drinkDescription = "Coca Cola";
        private readonly int _drinkMenuNumber = 234;
        private readonly decimal _drinkPrice = 2.5m;
        private readonly Guid _aggregateId = new Guid("8E8E2FE0-C223-456C-84ED-A6611AA5F03B");
        private ICommandHandlerFactory _commandHandlerFactory;
        private IEventRepository _eventRepository;
        private MarkDrinksServedCommand _markDrinksServedCommand;
        private Tab _tab;
        private ITabFactory _tabFactory;
        private IEventApplier _eventApplier;
        private IEnumerable<IEvent> _events;
        private TabOpened _tabOpened;

        [SetUp]
        public void SetUp()
        {
            SetUpTabFactory();
            SetUpEventStore();
            _eventApplier = Substitute.For<IEventApplier>();
            _commandHandlerFactory = new CommandHandlerFactory(_tabFactory, _eventRepository, _eventApplier);
            _markDrinksServedCommand = GetMarkDrinksServedCommand();
        }

        [Test]
        public void Retrieves_tab_to_handle_MarkDrinksServed_command()
        {
            WhenMarkingDrinksAsServed();

            Assert.That(_tab.Id, Is.EqualTo(_aggregateId));
        }

        [Test]
        public void Retrieved_tab_has_all_previous_events_applied()
        {
            WhenMarkingDrinksAsServed();

            _eventApplier.Received(1).ApplyEvent(_tabOpened, _tab);
            _eventApplier.Received(1).ApplyEvent(_drinksOrdered, _tab);
        }

        private void SetUpTabFactory()
        {
            var tab = new Tab {Id = _aggregateId};
            _tabFactory = Substitute.For<ITabFactory>();
            _tabFactory.Create(_aggregateId).Returns(tab);
        }

        private void SetUpEventStore()
        {
            _tabOpened = new TabOpened
            {
                Id = _tabOpenedEventId,
                AggregateId = _aggregateId,
                CommandId = _openTabCommandId,
                TableNumber = _tableNumber,
                Waiter = _waiter
            };
            _drinksOrdered = new DrinksOrdered
            {
                Id = _drinksOrderedId,
                AggregateId = _aggregateId,
                CommandId = _placeOrderCommandId,
                Items = new List<OrderedItem>
                {
                    new OrderedItem
                    {
                        Description = _drinkDescription,
                        IsDrink = true,
                        MenuNumber = _drinkMenuNumber,
                        Price = _drinkPrice
                    }
                }
            };
            _events = new List<IEvent>
            {
                _tabOpened,
                _drinksOrdered
            };
            _eventRepository = Substitute.For<IEventRepository>();
            _eventRepository.GetAllEventsFor(_aggregateId).Returns(_events);
        }

        private MarkDrinksServedCommand GetMarkDrinksServedCommand()
        {
            return new MarkDrinksServedCommand
            {
                Id = _markDrinksServedCommandId,
                AggregateId = _aggregateId,
                MenuNumbers = new List<int>
                {
                    _drinkMenuNumber
                }
            };
        }

        private void WhenMarkingDrinksAsServed()
        {
            var commandHandler = _commandHandlerFactory.CreateHandlerFor<IMarkDrinksServedCommand>(_markDrinksServedCommand);
            _tab = (Tab)commandHandler;
        }
    }
}
