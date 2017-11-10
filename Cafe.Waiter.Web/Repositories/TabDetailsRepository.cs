using System;
using System.Linq;
using Cafe.Waiter.Queries.DAL;
using Cafe.Waiter.Queries.DAL.Models;
using Newtonsoft.Json;

namespace Cafe.Waiter.Web.Repositories
{
    public class TabDetailsRepository: ITabDetailsRepository
    {
        private readonly IWaiterDbContext _waiterDbContext;

        public TabDetailsRepository(IWaiterDbContext waiterDbContext)
        {
            _waiterDbContext = waiterDbContext;
        }

        public TabDetails GetTabDetails(Guid id)
        {
            return Map(_waiterDbContext.TabDetails.Single(x => x.Id == id));
        }

        private TabDetails Map(Queries.DAL.Serialized.TabDetails tabDetails)
        {
            return JsonConvert.DeserializeObject<TabDetails>(tabDetails.Data);
        }
    }
}