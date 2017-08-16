using Cafe.Waiter.Web.DependencyInjection;
using MassTransit;
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
        public void BusControl_is_instantiated_as_singleton()
        {
            var busControl = Container.Instance.Resolve<IBusControl>();
            var busControl2 = Container.Instance.Resolve<IBusControl>();
            Assert.That(ReferenceEquals(busControl, busControl2));
        }
    }
}
