using System.Collections.Generic;
using NHibernate;

namespace Cafe.Waiter.DAL
{
    public class OpenTabsProvider
    {
        private readonly ISessionFactory _sessionFactory;

        public OpenTabsProvider(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public IEnumerable<OpenTab> GetOpenTabs()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    return session.QueryOver<OpenTab>().List();;
                }
            }
        }
    }
}