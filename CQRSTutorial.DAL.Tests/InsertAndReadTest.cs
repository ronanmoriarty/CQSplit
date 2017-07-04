using System.Data;
using NHibernate;
using NUnit.Framework;

namespace CQRSTutorial.DAL.Tests
{
    public abstract class InsertAndReadTest<TRepository>
        where TRepository : RepositoryBase
    {
        protected ISession WriteSession;
        protected TRepository Repository;

        [SetUp]
        public void SetUp()
        {
            WriteSession = SessionFactory.WriteInstance.OpenSession();
            WriteSession.BeginTransaction();
            Repository = CreateRepository(SessionFactory.ReadInstance, IsolationLevel.ReadUncommitted);
            Repository.UnitOfWork = new NHibernateUnitOfWork(WriteSession);
        }

        protected abstract TRepository CreateRepository(ISessionFactory readSessionFactory, IsolationLevel isolationLevel);

        [TearDown]
        public void TearDown()
        {
            WriteSession.Transaction.Rollback();
        }
    }
}