using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Waiter.Queries.DAL;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;
using Cafe.Waiter.Web.Repositories;
using CQRSTutorial.DAL.Tests.Common;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using MenuRepository = Cafe.Waiter.Web.Repositories.MenuRepository;

namespace Cafe.Waiter.Web.Tests.Repositories
{
    [TestFixture]
    public class MenuRepositoryTests
    {
        private Menu _menu;
        private readonly SqlExecutor _sqlExecutor = new SqlExecutor(ReadModelConnectionStringProvider.Instance);
        private MenuRepository _menuRepository;
        private readonly Guid _id = new Guid("35E02AF9-F608-47EE-A620-09E955C5ECB3");
        private int _menuItemId1 = 123;
        private string _menuItemName1 = "Coca Cola";
        private bool _menuItemIsDrink1 = true;
        private decimal _menuItemPrice1 = 2.5m;
        private int _menuItemId2 = 234;
        private string _menuItemName2 = "Bacon & Cheese Burger";
        private decimal _menuItemPrice2 = 13.0m;
        private bool _menuItemIsDrink2 = false;
        private IMenuConfiguration _menuConfiguration;

        [SetUp]
        public void SetUp()
        {
            _menu = GetMenu();
            var menuJson = JsonConvert.SerializeObject(_menu);
            _sqlExecutor.ExecuteNonQuery($@"DELETE FROM dbo.Menu WHERE Id = '{_id}'");
            _sqlExecutor.ExecuteNonQuery($@"INSERT INTO dbo.Menu(Id,Data) VALUES ('{_id}','{menuJson}')");
            _menuConfiguration = Substitute.For<IMenuConfiguration>();
            _menuConfiguration.Id.Returns(_id);
            _menuRepository = new MenuRepository(_menuConfiguration, ReadModelConnectionStringProvider.Instance);
        }

        [Test]
        public void Can_get_menu()
        {
            var retrievedMenu = _menuRepository.GetMenu();
            var firstMenuItem = retrievedMenu.Items.First();
            Assert.That(firstMenuItem.Id, Is.EqualTo(_menuItemId1));
            Assert.That(firstMenuItem.Name, Is.EqualTo(_menuItemName1));
            Assert.That(firstMenuItem.IsDrink, Is.EqualTo(_menuItemIsDrink1));
            Assert.That(firstMenuItem.Price, Is.EqualTo(_menuItemPrice1));

            var lastMenuItem = retrievedMenu.Items.Last();
            Assert.That(lastMenuItem.Id, Is.EqualTo(_menuItemId2));
            Assert.That(lastMenuItem.Name, Is.EqualTo(_menuItemName2));
            Assert.That(lastMenuItem.IsDrink, Is.EqualTo(_menuItemIsDrink2));
            Assert.That(lastMenuItem.Price, Is.EqualTo(_menuItemPrice2));
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
                        Name = _menuItemName1,
                        IsDrink = _menuItemIsDrink1,
                        Price = _menuItemPrice1
                    },
                    new MenuItem
                    {
                        Id = _menuItemId2,
                        Name = _menuItemName2,
                        IsDrink = _menuItemIsDrink2,
                        Price = _menuItemPrice2
                    }
                }
            };
        }
    }
}