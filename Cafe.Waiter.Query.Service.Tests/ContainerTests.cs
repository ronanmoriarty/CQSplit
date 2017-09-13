using Cafe.Waiter.Query.Service.Consumers;
using NUnit.Framework;

namespace Cafe.Waiter.Query.Service.Tests
{
    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void Can_resolve_WaiterQueryService()
        {
            var waiterQueryService = Container.Instance.Resolve<WaiterQueryService>();

            Assert.That(waiterQueryService, Is.Not.Null);
        }

        [Test]
        public void Can_resolve_tabOpenedConsumer()
        {
            var tabOpenedConsumer = Container.Instance.Resolve<TabOpenedConsumer>();

            Assert.That(tabOpenedConsumer, Is.Not.Null);
        }
    }
}
