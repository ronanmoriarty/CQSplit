using CQRSTutorial.Publisher;
using NUnit.Framework;

namespace Cafe.Waiter.Publish.Service.Tests
{
    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void CanResolvePublishService()
        {
            Bootstrapper.Initialize();

            var publishService = Container.Instance.Resolve<PublishService>();

            Assert.That(publishService, Is.Not.Null);
        }
    }
}
