using NHibernate;

namespace CQRSTutorial.DAL
{
    public class NHibernateUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly ISessionFactory _writeInstance;

        public NHibernateUnitOfWorkFactory(ISessionFactory writeInstance)
        {
            _writeInstance = writeInstance;
        }

        public IUnitOfWork Create()
        {
            return new NHibernateUnitOfWork(_writeInstance.OpenSession());
        }
    }
}