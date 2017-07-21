using System.Configuration;
using System.Data;
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
            var connectionString = ConfigurationManager.ConnectionStrings["CQRSTutorial"].ConnectionString;
            SessionFactory.WriteInstance = new NHibernateConfiguration(connectionString).CreateSessionFactory();
            SessionFactory.ReadInstance = new NHibernateConfiguration(connectionString).CreateSessionFactory(IsolationLevel.ReadUncommitted);
        }
    }
}
