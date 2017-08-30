using System.Collections.Generic;
using System.Linq;
using CQRSTutorial.DAL;
using Newtonsoft.Json;
using NHibernate;

namespace Cafe.Waiter.DAL
{
    public class OpenTabsRepository : RepositoryBase<OpenTab>, IOpenTabsRepository
    {
        private readonly ISessionFactory _sessionFactory;

        public OpenTabsRepository(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public IEnumerable<OpenTab> GetOpenTabs()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    return session.QueryOver<Serialized.OpenTab>().List().Select(Map);
                }
            }
        }

        private OpenTab Map(Serialized.OpenTab openTab)
        {
            return JsonConvert.DeserializeObject<OpenTab>(openTab.Data);
        }
    }
}