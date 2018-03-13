using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Waiter.Queries.DAL.Models;
using CQRSTutorial.DAL.Common;
using Newtonsoft.Json;

namespace Cafe.Waiter.Web.Repositories
{
    public class OpenTabsRepository : IOpenTabsRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public OpenTabsRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public IEnumerable<OpenTab> GetOpenTabs()
        {
            return GetAll().Select(Map);
        }

        private IEnumerable<Queries.DAL.Serialized.OpenTab> GetAll()
        {
            return GetWaiterDbContext().OpenTabs.ToList();
        }

        public Queries.DAL.Serialized.OpenTab Get(Guid id)
        {
            return GetWaiterDbContext().OpenTabs.SingleOrDefault(x => x.Id == id);
        }

        private WaiterDbContext GetWaiterDbContext()
        {
            return new WaiterDbContext(_connectionStringProvider.GetConnectionString());
        }

        private OpenTab Map(Queries.DAL.Serialized.OpenTab openTab)
        {
            return JsonConvert.DeserializeObject<OpenTab>(openTab.Data);
        }
    }
}