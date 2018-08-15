using Cafe.Waiter.EventProjecting.Service.Consumers;
using NUnit.Framework;

namespace Cafe.Waiter.EventProjecting.Service.Tests
{
    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void Can_resolve_EventProjectingService()
        {
            var eventProjectingService = Container.Instance.Resolve<EventProjectingService>();

            Assert.That(eventProjectingService, Is.Not.Null);
        }

        [Test]
        public void Can_resolve_tabOpenedConsumer()
        {
            var tabOpenedConsumer = Container.Instance.Resolve<TabOpenedEventConsumer>();

            Assert.That(tabOpenedConsumer, Is.Not.Null);
        }
    }
}
