using CQRSTutorial.DAL.Tests.Common;
using NHibernate;
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
            SessionFactory.Instance = Container.Instance.Resolve<ISessionFactory>();
        }
    }
}
