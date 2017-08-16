using Cafe.Waiter.Web.DependencyInjection;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests
{
    [SetUpFixture]
    public class RunOncePerTestRun
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ControllerLifestyleConfigurator.Instance = new TransientLifestyle();
            Bootstrapper.Start();
        }
    }
}