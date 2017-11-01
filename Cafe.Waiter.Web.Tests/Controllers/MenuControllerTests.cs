using System;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;
using Cafe.Waiter.Web.Controllers;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests.Controllers
{
    [TestFixture]
    public class MenuControllerTests
    {
        private MenuController _menuController;
        private readonly Guid _id = new Guid("39B35827-6A40-42D7-9114-8D1E297E9131");
        private Menu _menu;

        [SetUp]
        public void SetUp()
        {
            var menuRepository = Substitute.For<IMenuRepository>();
            var menu = new Menu
            {
                Id = _id
            };
            menuRepository.GetMenu().Returns(menu);
            _menuController = new MenuController(menuRepository);

            WhenGettingMenu();
        }

        [Test]
        public void Gets_menu_from_repository()
        {
            Assert.That(_menu.Id, Is.EqualTo(_id));
        }

        private void WhenGettingMenu()
        {
            _menu = _menuController.Get();
        }
    }
}
