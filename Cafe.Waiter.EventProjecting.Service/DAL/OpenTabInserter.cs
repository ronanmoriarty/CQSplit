using System;
using System.Linq;
using Cafe.Waiter.Queries.DAL;
using Cafe.Waiter.Queries.DAL.Models;
using Newtonsoft.Json;

namespace Cafe.Waiter.EventProjecting.Service.DAL
{
    public class OpenTabInserter : IOpenTabInserter
    {
        private readonly IWaiterDbContext _waiterDbContext;

        public OpenTabInserter(IWaiterDbContext waiterDbContext)
        {
            _waiterDbContext = waiterDbContext;
        }

        public void Insert(OpenTab openTab)
        {
            var existingOpenTab = Get(openTab.Id);
            if (existingOpenTab == null)
            {
                _waiterDbContext.AddOpenTab(new Queries.DAL.Serialized.OpenTab
                {
                    Id = openTab.Id,
                    Data = JsonConvert.SerializeObject(openTab)
                });
            }
        }

        private Queries.DAL.Serialized.OpenTab Get(Guid id)
        {
            return _waiterDbContext.OpenTabs.SingleOrDefault(x => x.Id == id);
        }
    }
}