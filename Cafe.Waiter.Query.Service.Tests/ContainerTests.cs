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
    }
}
