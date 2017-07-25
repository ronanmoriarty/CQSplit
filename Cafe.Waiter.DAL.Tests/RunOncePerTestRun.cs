using CQRSTutorial.DAL;
using CQRSTutorial.DAL.Tests.Common;
using NUnit.Framework;

namespace Cafe.Waiter.DAL.Tests
{
    [SetUpFixture]
    public class RunOncePerTestRun
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var connectionStringProviderFactory = new ConnectionStringProviderFactory("CQRSTutorial", "CQRS_CONNECTIONSTRING_OVERRIDE");
            var nHibernateConfiguration = new NHibernateConfiguration(connectionStringProviderFactory);
            SessionFactory.WriteInstance = nHibernateConfiguration.CreateSessionFactory();
            SessionFactory.ReadInstance = nHibernateConfiguration.CreateSessionFactory();
        }
    }
}