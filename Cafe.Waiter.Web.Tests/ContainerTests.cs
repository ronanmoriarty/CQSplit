using Cafe.Waiter.Web.DependencyInjection;
using CQRSTutorial.Infrastructure;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests
{
    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void Can_instantiate_TabController()
        {
            var tabController = Container.Instance.Resolve<Controllers.TabController>();

            Assert.That(tabController, Is.Not.Null);
        }

        [Test]
        public void Can_instantiate_MessageBusFactory()
        {
            var tabController = Container.Instance.Resolve<MessageBusFactory>();

            Assert.That(tabController, Is.Not.Null);
        }
    }
}
