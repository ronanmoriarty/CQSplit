using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Waiter.Commands;
using Cafe.Waiter.Contracts.Commands;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;
using CQRSTutorial.Messaging;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests.Api.TabController
{
    [TestFixture]
    public class When_placing_order
    {
        private Web.Controllers.TabController _tabController;
        private ICommandSender _commandSender;
        private IMenuRepository _menuRepository;
        private readonly Guid _id = new Guid("41D245F0-192C-4381-9A93-0EF97D005460");
        private readonly TabStatus _tabStatus = TabStatus.Seated;
        private readonly int _tableNumber = 123;
        private readonly string _waiter = "Mary";
        private int _menuItemId1 = 123;
        private bool _isDrink1 = false;
        private string _name1 = "Tikka Masala";
        private string _notes1 = "abc";
        private decimal _price1 = 50m;
        private int _menuItemId2 = 234;
        private bool _isDrink2 = true;
        private string _name2 = "Lemonade";
        private string _notes2 = "def";
        private decimal _price2 = 100m;

        [SetUp]
        public void SetUp()
        {
            _commandSender = Substitute.For<ICommandSender>();
            _menuRepository = Substitute.For<IMenuRepository>();
            _menuRepository.GetMenu().Returns(GetMenu());
            _tabController = new Web.Controllers.TabController(null, null, _commandSender, new Web.Controllers.PlaceOrderCommandFactory(_menuRepository));

            _tabController.PlaceOrder(GetTabDetails());
        }

        [Test]
        public void PlaceOrder_command_sent()
        {
            _commandSender.Received(1).Send(Arg.Is<PlaceOrderCommand>(command => MatchingCommand(command)));
        }

        private Menu GetMenu()
        {
            return new Menu
            {
                Items = new List<MenuItem>
                {
                    new MenuItem
                    {
                        Id = _menuItemId1,
                        IsDrink = _isDrink1,
                        Name = _name1,
                        Price = _price1
                    },
                    new MenuItem
                    {
                        Id = _menuItemId2,
                        IsDrink = _isDrink2,
                        Name = _name2,
                        Price = _price2
                    }
                }
            };
        }

        private TabDetails GetTabDetails()
        {
            return new TabDetails
            {
                Id = _id,
                Status = _tabStatus,
                TableNumber = _tableNumber,
                Waiter = _waiter,
                Items = new[]
                {
                    new TabItem
                    {
                        MenuNumber = _menuItemId1,
                        IsDrink = _isDrink1,
                        Name = _name1,
                        Notes = _notes1
                    },
                    new TabItem
                    {
                        MenuNumber = _menuItemId2,
                        IsDrink = _isDrink2,
                        Name = _name2,
                        Notes = _notes2
                    }
                }
            };
        }

        private bool MatchingCommand(PlaceOrderCommand command)
        {
            Assert.That(command.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(command.AggregateId, Is.EqualTo(_id));
            AssertItemsMatch(command.Items);
            return true;
        }

        private void AssertItemsMatch(List<OrderedItem> commandItems)
        {
            Assert.That(commandItems.Count, Is.EqualTo(2));
            var first = commandItems.First();
            Assert.That(first.MenuNumber, Is.EqualTo(_menuItemId1));
            Assert.That(first.Description, Is.EqualTo(_name1));
            Assert.That(first.IsDrink, Is.EqualTo(_isDrink1));
            Assert.That(first.Notes, Is.EqualTo(_notes1));
            Assert.That(first.Price, Is.EqualTo(_price1));

            var second = commandItems.Last();
            Assert.That(second.MenuNumber, Is.EqualTo(_menuItemId2));
            Assert.That(second.Description, Is.EqualTo(_name2));
            Assert.That(second.IsDrink, Is.EqualTo(_isDrink2));
            Assert.That(second.Notes, Is.EqualTo(_notes2));
            Assert.That(second.Price, Is.EqualTo(_price2));
        }
    }
}
