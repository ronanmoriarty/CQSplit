using NUnit.Framework;

namespace Cafe.Waiter.Query.Service.Tests
{
    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void Can_resolve_waiter_query_service()
        {
            Container.Instance.Resolve(typeof(WaiterQueryService));
        }
    }
}
