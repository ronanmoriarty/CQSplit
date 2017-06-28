using NHibernate;

namespace CQRSTutorial.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISessionFactory _sessionFactory;
        private ISession _session;

        public UnitOfWork(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public ISession GetSession()
        {
            if (_session == null)
            {
                _session = _sessionFactory.OpenSession();
                _session.BeginTransaction();
            }

            return _session;
        }

        public void Commit()
        {
            _session?.Transaction?.Commit();
        }

        public void Rollback()
        {
            if (_session != null && _session.IsOpen)
            {
                _session?.Transaction?.Rollback();
            }
        }
    }
}