using System;
using System.Linq;
using Cafe.Waiter.Queries.DAL.Models;
using Newtonsoft.Json;

namespace Cafe.Waiter.Queries.DAL.Repositories
{
    public class TabDetailsRepository: ITabDetailsRepository
    {
        public TabDetails GetTabDetails(Guid id)
        {
            return Map(new WaiterDbContext(ReadModelConnectionStringProviderFactory.Instance.GetConnectionStringProvider().GetConnectionString()).TabDetails.Single(x => x.Id == id));
        }

        private TabDetails Map(Serialized.TabDetails tabDetails)
        {
            return JsonConvert.DeserializeObject<TabDetails>(tabDetails.Data);
        }
    }
}