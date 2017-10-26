using log4net.Config;
using NUnit.Framework;

namespace CQRSTutorial.Publisher.Tests
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