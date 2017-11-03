using log4net.Config;
using NUnit.Framework;

namespace CQRSTutorial.Publish.Tests
{
    [SetUpFixture]
    public class RunOncePerTestRun
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            XmlConfigurator.Configure();
        }
    }
}