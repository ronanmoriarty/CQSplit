using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Waiter.Queries.DAL.Models;
using Newtonsoft.Json;

namespace Cafe.Waiter.Web.Repositories
{
    public class OpenTabsRepository : IOpenTabsRepository
    {
        private readonly string _connectionString;

        public OpenTabsRepository(string connectionString)
        {
            _connectionString = connectionString;
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
            return new WaiterDbContext(_connectionString);
        }

        private OpenTab Map(Queries.DAL.Serialized.OpenTab openTab)
        {
            return JsonConvert.DeserializeObject<OpenTab>(openTab.Data);
        }
    }
}