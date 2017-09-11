using NUnit.Framework;

namespace Cafe.Waiter.Publish.Service.Tests
{
    [SetUpFixture]
    public class RunOncePerTestRun
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Bootstrapper.Start();
        }
    }
}
