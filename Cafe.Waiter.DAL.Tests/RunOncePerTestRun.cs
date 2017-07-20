using System.Configuration;
using System.Data;
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
            var connectionString = ConfigurationManager.ConnectionStrings["CQRSTutorial"].ConnectionString;
            SessionFactory.WriteInstance = NHibernateConfiguration.CreateSessionFactory(connectionString);
            SessionFactory.ReadInstance = NHibernateConfiguration.CreateSessionFactory(connectionString, IsolationLevel.ReadUncommitted);
        }
    }
}