using CQRSTutorial.DAL.Tests.Common;
using log4net.Config;
using NUnit.Framework;

namespace CQRSTutorial.DAL.Tests
{
    [SetUpFixture]
    public class RunOncePerTestRun
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            XmlConfigurator.Configure();
            var connectionStringProviderFactory = WriteModelConnectionStringProviderFactory.Instance;
            var nHibernateConfiguration = new NHibernateConfiguration(connectionStringProviderFactory);
            SessionFactory.Instance = nHibernateConfiguration.CreateSessionFactory();
        }
    }
}
