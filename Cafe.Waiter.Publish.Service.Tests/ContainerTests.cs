using NUnit.Framework;

namespace Cafe.Waiter.Publish.Service.Tests
{
    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void CanResolvePublishService()
        {
            var publishService = Container.Instance.Resolve<PublishService>();
            var publishService2 = Container.Instance.Resolve<PublishService>();

            Assert.That(publishService, Is.Not.Null);
            AssertInstantiatedAsSingleton(publishService, publishService2);
        }

        private static void AssertInstantiatedAsSingleton(PublishService publishService, PublishService publishService2)
        {
            Assert.That(publishService, Is.EqualTo(publishService2));
        }
    }
}
