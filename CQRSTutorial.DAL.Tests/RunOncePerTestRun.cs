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
            SessionFactory.WriteInstance = NHibernateConfiguration.CreateSessionFactory();
            SessionFactory.ReadInstance = NHibernateConfiguration.CreateSessionFactory(IsolationLevel.ReadUncommitted);
        }
    }
}
