using System;
using System.Linq;
using Cafe.Waiter.Queries.DAL.Models;
using CQRSTutorial.DAL.Common;
using log4net;
using Newtonsoft.Json;

namespace Cafe.Waiter.EventProjecting.Service.DAL
{
    public class OpenTabInserter : IOpenTabInserter
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly ILog _logger = LogManager.GetLogger(typeof(OpenTabInserter));

        public OpenTabInserter(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public void Insert(OpenTab openTab)
        {
            var existingOpenTab = Get(openTab.Id);
            if (existingOpenTab == null)
            {
                _logger.Debug($"Inserting new OpenTab record with Id {openTab.Id}...");
                var waiterDbContext = GetWaiterDbContext();
                waiterDbContext.OpenTabs.Add(new Queries.DAL.Serialized.OpenTab
                {
                    Id = openTab.Id,
                    Data = JsonConvert.SerializeObject(openTab)
                });
                waiterDbContext.SaveChanges();
            }
            else
            {
                throw new ArgumentException($"Tab with id {openTab.Id} already exists in dbo.OpenTabs.");
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