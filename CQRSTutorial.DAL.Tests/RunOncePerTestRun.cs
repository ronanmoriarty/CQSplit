using System.Data;
using NUnit.Framework;

namespace CQRSTutorial.DAL.Tests
{
    [SetUpFixture]
    public class RunOncePerTestRun
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            SessionFactory.Instance = NHibernateConfiguration.CreateSessionFactory();
            SessionFactory.InstanceForReadingUncommittedChanges = NHibernateConfiguration.CreateSessionFactory(IsolationLevel.ReadUncommitted);
        }
    }
}
