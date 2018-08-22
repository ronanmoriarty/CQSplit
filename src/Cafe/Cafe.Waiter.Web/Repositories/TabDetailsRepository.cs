using System;
using System.Linq;
using Cafe.Waiter.Queries.DAL.Models;
using Newtonsoft.Json;

namespace Cafe.Waiter.Web.Repositories
{
    public class TabDetailsRepository: ITabDetailsRepository
    {
        private readonly string _connectionString;

        public TabDetailsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public TabDetails GetTabDetails(Guid id)
        {
            using (var waiterDbContext = new WaiterDbContext(_connectionString))
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