using NUnit.Framework;

namespace Cafe.Waiter.Command.Service.IntegrationTests
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
