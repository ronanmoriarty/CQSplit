using Cafe.Waiter.Web.Controllers;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests
{
    [TestFixture]
    public class StartupTests
    {
        [Test]
        public void Can_resolve_ValuesController()
        {
            var valuesController = (ValuesController)BuildServiceProvider.Instance.GetService(typeof(ValuesController));

            Assert.That(valuesController, Is.Not.Null);
        }

        [Test]
        public void Can_resolve_MenuController()
        {
            var menuController = (MenuController)BuildServiceProvider.Instance.GetService(typeof(MenuController));

            Assert.That(menuController, Is.Not.Null);
        }
    }
}
