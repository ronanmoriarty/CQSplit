using NUnit.Framework;

namespace Cafe.Waiter.Command.Service.Tests
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
