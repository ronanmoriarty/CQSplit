using System;
using Cafe.Waiter.Queries.DAL.Models;
using CQRSTutorial.DAL;
using Newtonsoft.Json;
using NHibernate;

namespace Cafe.Waiter.Queries.DAL.Repositories
{
    public class TabDetailsRepository : RepositoryBase<Serialized.TabDetails>, ITabDetailsRepository
    {
        public TabDetailsRepository(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
        }

        public TabDetails GetTabDetails(Guid id)
        {
            var serializedTabDetails = Get(id);
            return Map(serializedTabDetails);
        }

        private TabDetails Map(Serialized.TabDetails tabDetails)
        {
            return JsonConvert.DeserializeObject<TabDetails>(tabDetails.Data);
        }
    }
}