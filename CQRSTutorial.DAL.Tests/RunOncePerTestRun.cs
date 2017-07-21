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
            SessionFactory.WriteInstance = new NHibernateConfiguration().CreateSessionFactory(connectionString);
            SessionFactory.ReadInstance = new NHibernateConfiguration().CreateSessionFactory(connectionString, IsolationLevel.ReadUncommitted);
        }
    }
}
