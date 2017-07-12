using NHibernate;

namespace CQRSTutorial.DAL
{
    public class NHibernateUnitOfWork : IUnitOfWork
    {
        public NHibernateUnitOfWork(ISession session)
        {
            Session = session;
        }

        public ISession Session { get; }

        public void Dispose()
        {
            Session?.Dispose();
        }

        public void Start()
        {
            Session?.BeginTransaction();
        }

        public void Commit()
        {
            Session?.Transaction?.Commit();
        }

        public void Rollback()
        {
            Session?.Transaction?.Rollback();
        }
    }
}