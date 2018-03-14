using System;
using System.Linq;
using Cafe.Waiter.Queries.DAL.Models;
using CQRSTutorial.DAL.Common;
using Newtonsoft.Json;

namespace Cafe.Waiter.EventProjecting.Service.DAL
{
    public class OpenTabInserter : IOpenTabInserter
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public OpenTabInserter(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public void Insert(OpenTab openTab)
        {
            var existingOpenTab = Get(openTab.Id);
            if (existingOpenTab == null)
            {
                var waiterDbContext = GetWaiterDbContext();
                waiterDbContext.OpenTabs.Add(new Queries.DAL.Serialized.OpenTab
                {
                    Id = openTab.Id,
                    Data = JsonConvert.SerializeObject(openTab)
                });
                waiterDbContext.SaveChanges();
            }
        }

        private Queries.DAL.Serialized.OpenTab Get(Guid id)
        {
            return GetWaiterDbContext().OpenTabs.SingleOrDefault(x => x.Id == id);
        }

        private WaiterDbContext GetWaiterDbContext()
        {
            return new WaiterDbContext(_connectionStringProvider.GetConnectionString());
        }
    }
}