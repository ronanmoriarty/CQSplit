using NUnit.Framework;

namespace Cafe.Waiter.Service.Tests
{
    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void Can_resolve_WaiterService()
        {
            var waiterService = Container.Instance.Resolve<WaiterService>();

            Assert.That(waiterService, Is.Not.Null);
        }
    }
}
