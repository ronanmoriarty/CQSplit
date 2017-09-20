using System;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests.Api.MenuController
{
    [TestFixture]
    public class When_getting_menu
    {
        private Web.Api.MenuController _menuController;
        private readonly Guid _id = new Guid("39B35827-6A40-42D7-9114-8D1E297E9131");
        private Menu _menu;
        private string _contentType;

        [SetUp]
        public void SetUp()
        {
            var menuRepository = Substitute.For<IMenuRepository>();
            var menu = new Menu
            {
                Id = _id
            };
            menuRepository.GetMenu().Returns(menu);
            _menuController = new Web.Api.MenuController(menuRepository);

            WhenGettingMenu();
        }

        [Test]
        public void Gets_menu_from_repository()
        {
            Assert.That(_menu.Id, Is.EqualTo(_id));
        }

        [Test]
        public void Content_type_is_json()
        {
            Assert.That(_contentType, Is.EqualTo("application/json"));
        }

        private void WhenGettingMenu()
        {
            var contentResult = _menuController.Index();
            _contentType = contentResult.ContentType;
            var content = contentResult.Content;
            _menu = JsonConvert.DeserializeObject<Menu>(content);
        }
    }
}
