using System;
using System.Data;
using NHibernate;

namespace CQRSTutorial.DAL
{
    public abstract class RepositoryBase
    {
        private readonly ISessionFactory _readSessionFactory;
        private readonly IsolationLevel _isolationLevel;

        protected RepositoryBase(ISessionFactory readSessionFactory, IsolationLevel isolationLevel)
        {
            _readSessionFactory = readSessionFactory;
            _isolationLevel = isolationLevel;
        }

        public ISession WriteSession { get; set; }

        protected void SaveOrUpdate(object objectToSave)
        {
            WriteSession.SaveOrUpdate(objectToSave);
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
    }
}