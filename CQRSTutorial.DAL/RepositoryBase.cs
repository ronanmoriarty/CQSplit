using System.Data;
using NHibernate;

namespace CQRSTutorial.DAL
{
    public abstract class RepositoryBase<TTypeToPersist> where TTypeToPersist : class
    {
        protected readonly ISessionFactory ReadSessionFactory;
        protected readonly IsolationLevel IsolationLevel;

        protected RepositoryBase(ISessionFactory readSessionFactory, IsolationLevel isolationLevel)
        {
            ReadSessionFactory = readSessionFactory;
            IsolationLevel = isolationLevel;
        }

        public object UnitOfWork { get; set; }

        protected void SaveOrUpdate(TTypeToPersist objectToSave)
        {
            ((NHibernateUnitOfWork)UnitOfWork).Session.SaveOrUpdate(objectToSave);
        }

        protected TTypeToPersist Get(int id)
        {
            using (var session = ReadSessionFactory.OpenSession())
            {
                using (session.BeginTransaction(IsolationLevel))
                {
                    return session.Get<TTypeToPersist>(id);
                }
            }
        }
    }
}