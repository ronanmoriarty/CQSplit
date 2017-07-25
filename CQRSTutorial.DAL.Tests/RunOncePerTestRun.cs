using CQRSTutorial.DAL.Tests.Common;
using NUnit.Framework;

namespace CQRSTutorial.DAL.Tests
{
    [SetUpFixture]
    public class RunOncePerTestRun
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var connectionStringProviderFactory = new ConnectionStringProviderFactory("CQRSTutorial", "CQRS_CONNECTIONSTRING_OVERRIDE");
            var nHibernateConfiguration = new NHibernateConfiguration(connectionStringProviderFactory);
            SessionFactory.Instance = nHibernateConfiguration.CreateSessionFactory();
        }
    }
}
