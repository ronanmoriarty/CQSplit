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
            SessionFactory.WriteInstance = NHibernateConfiguration.CreateSessionFactory();
            SessionFactory.ReadInstance = NHibernateConfiguration.CreateSessionFactory(IsolationLevel.ReadUncommitted);
        }
    }
}