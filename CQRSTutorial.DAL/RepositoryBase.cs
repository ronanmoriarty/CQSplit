using NHibernate;

namespace CQRSTutorial.DAL
{
    public abstract class RepositoryBase<TTypeToPersist> where TTypeToPersist : class
    {
        protected readonly ISessionFactory SessionFactory;

        protected RepositoryBase(ISessionFactory sessionFactory)
        {
            SessionFactory = sessionFactory;
        }

        public IUnitOfWork UnitOfWork { get; set; } // TODO really don't like this property and the way the following method relies on it being set. Figure out a better way to deal with this.

        public void SaveOrUpdate(TTypeToPersist objectToSave)
        {
            ((NHibernateUnitOfWork)UnitOfWork).Session.SaveOrUpdate(objectToSave);
        }

        public TTypeToPersist Get(int id)
        {
            using (var session = SessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    return session.Get<TTypeToPersist>(id);
                }
            }
        }

        public void Delete(TTypeToPersist objectToDelete)
        {
            ((NHibernateUnitOfWork)UnitOfWork).Session.Delete(objectToDelete);
        }
    }
}