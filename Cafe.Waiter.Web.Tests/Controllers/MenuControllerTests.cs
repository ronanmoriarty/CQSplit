using System;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Web.Controllers;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests.Controllers
{
    [TestFixture]
    public class MenuControllerTests
    {
        private MenuController _menuController;
        private readonly Guid _id = new Guid("68FDB1AF-B652-4B37-B274-D1C7F569FFE7");
        private Menu _menu;

        [SetUp]
        public void SetUp()
        {
            _menuController = (MenuController)BuildServiceProvider.Instance.GetService(typeof(MenuController));

            WhenGettingMenu();
        }

        [Test]
        public void Gets_menu_using_id_from_config()
        {
            Assert.That(_menu.Id, Is.EqualTo(_id));
        }

        private void WhenGettingMenu()
        {
            _menu = _menuController.Get();
        }
    }
}
