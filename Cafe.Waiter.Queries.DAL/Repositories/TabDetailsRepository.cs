using System;
using System.Linq;
using Cafe.Waiter.Queries.DAL.Models;
using CQRSTutorial.DAL;
using Newtonsoft.Json;

namespace Cafe.Waiter.Queries.DAL.Repositories
{
    public class TabDetailsRepository: ITabDetailsRepository
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public TabDetailsRepository(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public TabDetails GetTabDetails(Guid id)
        {
            return Map(new WaiterDbContext(_connectionStringProvider.GetConnectionString()).TabDetails.Single(x => x.Id == id));
        }

        private TabDetails Map(Serialized.TabDetails tabDetails)
        {
            return JsonConvert.DeserializeObject<TabDetails>(tabDetails.Data);
        }
    }
}