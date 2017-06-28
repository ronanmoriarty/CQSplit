using NHibernate;

namespace CQRSTutorial.DAL
{
    public interface IUnitOfWork
    {
        ISession GetSession();
        void Commit();
        void Rollback();
    }
}