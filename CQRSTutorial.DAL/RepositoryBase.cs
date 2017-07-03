using System;
using System.Data;
using NHibernate;

namespace CQRSTutorial.DAL
{
    public abstract class RepositoryBase
    {
        private readonly ISessionFactory _writeSessionFactory;
        private readonly ISessionFactory _readSessionFactory;
        private readonly IsolationLevel _isolationLevel;
        private ISession _session;

        protected RepositoryBase(ISessionFactory writeSessionFactory, ISessionFactory readSessionFactory, IsolationLevel isolationLevel)
        {
            _writeSessionFactory = writeSessionFactory;
            _readSessionFactory = readSessionFactory;
            _isolationLevel = isolationLevel;
        }

        public void BeginTransaction()
        {
            _session = _writeSessionFactory.OpenSession();
            _session.BeginTransaction();
        }

        protected void SaveOrUpdate(object objectToSave)
        {
            _session.SaveOrUpdate(objectToSave);
            _session.Flush();
        }

        protected TReturnType Get<TReturnType>(Guid id)
        {
            using (var session = _readSessionFactory.OpenSession())
            {
                using (session.BeginTransaction(_isolationLevel))
                {
                    return session.Get<TReturnType>(id);
                }
            }
        }

        public void Rollback()
        {
            _session.Transaction.Rollback();
        }
    }
}