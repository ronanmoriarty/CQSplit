using Cafe.Waiter.Web.DependencyInjection;
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
    }
}
