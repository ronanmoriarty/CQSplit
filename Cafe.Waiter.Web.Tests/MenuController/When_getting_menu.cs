using System;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests.MenuController
{
    [TestFixture]
    public class When_getting_menu
    {
        private Controllers.MenuController _menuController;
        private Menu _menu;
        private readonly Guid _id = new Guid("39B35827-6A40-42D7-9114-8D1E297E9131");

        [SetUp]
        public void SetUp()
        {
            var menuRepository = Substitute.For<IMenuRepository>();
            _menu = new Menu
            {
                Id = _id
            };
            menuRepository.GetMenu().Returns(_menu);
            _menuController = new Controllers.MenuController(menuRepository);
        }

        [Test]
        public void Gets_menu_from_repository()
        {
            var menuContentResult = _menuController.Index();

            var content = menuContentResult.Content;
            var retrievedMenu = JsonConvert.DeserializeObject<Menu>(content);
            Assert.That(retrievedMenu.Id, Is.EqualTo(_menu.Id));
        }
    }
}
