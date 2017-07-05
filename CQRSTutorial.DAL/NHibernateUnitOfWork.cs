using NHibernate;

namespace CQRSTutorial.DAL
{
    public class NHibernateUnitOfWork
    {
        public NHibernateUnitOfWork(ISession session)
        {
            Session = session;
        }

        public ISession Session { get; private set; }
    }
}