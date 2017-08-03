using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests
{
    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void Can_instantiate_MessageBus()
        {
            var messageBus = Container.Instance.Resolve<IMessageBus>();

            Assert.That(messageBus, Is.Not.Null);
        }
    }
}
