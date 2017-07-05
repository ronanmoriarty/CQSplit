using System;
using System.Data;
using NHibernate;

namespace CQRSTutorial.DAL
{
    public abstract class RepositoryBase<TTypeToPersist>
    {
        private readonly ISessionFactory _readSessionFactory;
        private readonly IsolationLevel _isolationLevel;

        protected RepositoryBase(ISessionFactory readSessionFactory, IsolationLevel isolationLevel)
        {
            _readSessionFactory = readSessionFactory;
            _isolationLevel = isolationLevel;
        }

        public object UnitOfWork { get; set; }

        protected void SaveOrUpdate(TTypeToPersist objectToSave)
        {
            ((NHibernateUnitOfWork)UnitOfWork).Session.SaveOrUpdate(objectToSave);
        }

        protected TTypeToPersist Get(int id)
        {
            using (var session = _readSessionFactory.OpenSession())
            {
                using (session.BeginTransaction(_isolationLevel))
                {
                    return session.Get<TTypeToPersist>(id);
                }
            }
        }
    }
}