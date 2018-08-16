using NUnit.Framework;

namespace Cafe.Waiter.EventProjecting.Service.IntegrationTests
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