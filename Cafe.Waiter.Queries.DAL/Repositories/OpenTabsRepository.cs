using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Waiter.Queries.DAL.Models;
using CQRSTutorial.DAL.Common;
using Newtonsoft.Json;

namespace Cafe.Waiter.Queries.DAL.Repositories
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

        private IEnumerable<Serialized.OpenTab> GetAll()
        {
            return CreateWaiterDbContext().OpenTabs.ToList();
        }

        public Serialized.OpenTab Get(Guid id)
        {
            return CreateWaiterDbContext().OpenTabs.SingleOrDefault(x => x.Id == id);
        }

        public void Insert(OpenTab openTab)
        {
            var existingOpenTab = Get(openTab.Id);
            if (existingOpenTab == null)
            {
                var waiterDbContext = CreateWaiterDbContext();
                waiterDbContext.OpenTabs.Add(new Serialized.OpenTab
                {
                    Id = openTab.Id,
                    Data = JsonConvert.SerializeObject(openTab)
                });
                waiterDbContext.SaveChanges();
            }
        }

        private WaiterDbContext CreateWaiterDbContext()
        {
            return new WaiterDbContext(_connectionStringProvider.GetConnectionString());
        }

        private OpenTab Map(Serialized.OpenTab openTab)
        {
            return JsonConvert.DeserializeObject<OpenTab>(openTab.Data);
        }
    }
}