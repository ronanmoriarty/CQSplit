using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Web.Controllers;
using Cafe.Waiter.Web.Repositories;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests.Controllers
{
    [TestFixture]
    public class MenuControllerTests
    {
        private MenuController _menuController;
        private Menu _menuFromRepository;
        private IMenuRepository _menuRepository;

        [SetUp]
        public void SetUp()
        {
            _menuRepository = Substitute.For<IMenuRepository>();
            _menuFromRepository = new Menu();
            _menuRepository.GetMenu().Returns(_menuFromRepository);
            _menuController = new MenuController(_menuRepository);
        }

        [Test]
        public void Gets_menu_from_repository()
        {
            var menu = _menuController.Get();

            Assert.That(menu, Is.EqualTo(_menuFromRepository));
        }
    }
}
