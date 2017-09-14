using System;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests.MenuController
{
    [TestFixture]
    public class When_getting_menu
    {
        private Controllers.MenuController _menuController;
        private Menu _menu;

        [SetUp]
        public void SetUp()
        {
            var menuRepository = Substitute.For<IMenuRepository>();
            _menu = new Menu();
            menuRepository.GetMenu().Returns(_menu);
            _menuController = new Controllers.MenuController(menuRepository);
        }

        [Test]
        public void Gets_menu_from_repository()
        {
            var retrievedMenu = _menuController.Get();

            Assert.That(retrievedMenu, Is.EqualTo(_menu));
        }
    }
}
