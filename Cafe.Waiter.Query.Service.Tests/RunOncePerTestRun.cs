using NUnit.Framework;

namespace Cafe.Waiter.Query.Service.Tests
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