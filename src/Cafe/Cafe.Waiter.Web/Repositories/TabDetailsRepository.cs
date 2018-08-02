using System;
using System.Linq;
using Cafe.DAL.Common;
using Cafe.Waiter.Queries.DAL.Models;
using Newtonsoft.Json;

namespace Cafe.Waiter.Web.Repositories
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
            using (var waiterDbContext = new WaiterDbContext(_connectionStringProvider.GetConnectionString()))
            {
                var serializedTabDetails = waiterDbContext.TabDetails.SingleOrDefault(x => x.Id == id);
                if (serializedTabDetails == null)
                {
                    return null;
                }

                return Map(serializedTabDetails);
            }
        }

        private TabDetails Map(Queries.DAL.Serialized.TabDetails tabDetails)
        {
            return JsonConvert.DeserializeObject<TabDetails>(tabDetails.Data);
        }
    }
}