using CQRSTutorial.DAL;
using CQRSTutorial.DAL.Tests;
using CQRSTutorial.DAL.Tests.Common;
using NUnit.Framework;

namespace CQRSTutorial.Publisher.Tests
{
    [SetUpFixture]
    public class RunOncePerTestRun
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var connectionStringProviderFactory = WriteModelConnectionStringProviderFactory.Instance;
            var nHibernateConfiguration = new NHibernateConfiguration(connectionStringProviderFactory);
            SessionFactory.Instance = nHibernateConfiguration.CreateSessionFactory();
        }
    }
}