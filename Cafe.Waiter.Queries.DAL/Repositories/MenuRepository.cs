using CQRSTutorial.DAL;
using NHibernate;

namespace Cafe.Waiter.Queries.DAL.Repositories
{
    public class MenuRepository : RepositoryBase<Serialized.TabDetails>, IMenuRepository
    {
        public MenuRepository(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
        }
    }
}