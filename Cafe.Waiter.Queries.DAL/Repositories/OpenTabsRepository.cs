﻿using System.Collections.Generic;
using System.Linq;
using Cafe.Waiter.Queries.DAL.Models;
using CQRSTutorial.DAL;
using Newtonsoft.Json;
using NHibernate;

namespace Cafe.Waiter.Queries.DAL.Repositories
{
    public class OpenTabsRepository : RepositoryBase<Serialized.OpenTab>, IOpenTabsRepository
    {
        public OpenTabsRepository(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
        }

        public IEnumerable<OpenTab> GetOpenTabs()
        {
            return GetAll().Select(Map);
        }

        public void Insert(OpenTab openTab)
        {
            throw new System.NotImplementedException();
        }

        private OpenTab Map(Serialized.OpenTab openTab)
        {
            return JsonConvert.DeserializeObject<OpenTab>(openTab.Data);
        }
    }
}