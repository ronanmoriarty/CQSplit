using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.NHibernate;
using Cafe.Waiter.Queries.DAL.Repositories;
using CQRSTutorial.DAL.Tests.Common;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Cafe.Waiter.Queries.DAL.Tests.Repositories
{
    [TestFixture]
    public class MenuRepositoryTests
    {
        private Menu _menu;
        private readonly SqlExecutor _sqlExecutor = new SqlExecutor(ReadModelConnectionStringProviderFactory.Instance);
        private MenuRepository _menuRepository;
        private readonly Guid _id = new Guid("35E02AF9-F608-47EE-A620-09E955C5ECB3");
        private int _menuItemId1 = 123;
        private string _menuItemName1 = "Coca Cola";
        private decimal _menuItemPrice1 = 2.5m;
        private int _menuItemId2 = 234;
        private string _menuItemName2 = "Bacon & Cheese Burger";
        private decimal _menuItemPrice2 = 13.0m;

        [SetUp]
        public void SetUp()
        {
            _menu = GetMenu();
            var menuJson = JsonConvert.SerializeObject(_menu);
            _sqlExecutor.ExecuteNonQuery($@"DELETE FROM dbo.Menu WHERE Id = '{_id}'");
            _sqlExecutor.ExecuteNonQuery($@"INSERT INTO dbo.Menu(Id,Data) VALUES ('{_id}','{menuJson}')");
            _menuRepository = new MenuRepository(ReadModelSessionFactory.Instance);
        }

        [Test]
        public void Can_get_menu()
        {
            var retrievedMenu = _menuRepository.GetMenu(_id);

            Assert.That(retrievedMenu.Items.First().Id, Is.EqualTo(_menuItemId1));
            Assert.That(retrievedMenu.Items.First().Name, Is.EqualTo(_menuItemName1));
            Assert.That(retrievedMenu.Items.First().Price, Is.EqualTo(_menuItemPrice1));

            Assert.That(retrievedMenu.Items.Last().Id, Is.EqualTo(_menuItemId2));
            Assert.That(retrievedMenu.Items.Last().Name, Is.EqualTo(_menuItemName2));
            Assert.That(retrievedMenu.Items.Last().Price, Is.EqualTo(_menuItemPrice2));
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
                        Price = _menuItemPrice1
                    },
                    new MenuItem
                    {
                        Id = _menuItemId2,
                        Name = _menuItemName2,
                        Price = _menuItemPrice2
                    }
                }
            };
        }
    }
}