using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;
using Newtonsoft.Json;

namespace Cafe.Waiter.EventProjecting.Service
{
    public class OpenTabsRepository : IOpenTabsRepository
    {
        private readonly IWaiterDbContext _waiterDbContext;

        public OpenTabsRepository(IWaiterDbContext waiterDbContext)
        {
            _waiterDbContext = waiterDbContext;
        }

        public IEnumerable<OpenTab> GetOpenTabs()
        {
            return GetAll().Select(Map);
        }

        private IEnumerable<Queries.DAL.Serialized.OpenTab> GetAll()
        {
            return _waiterDbContext.OpenTabs.ToList();
        }

        public Queries.DAL.Serialized.OpenTab Get(Guid id)
        {
            return _waiterDbContext.OpenTabs.SingleOrDefault(x => x.Id == id);
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

        private OpenTab Map(Queries.DAL.Serialized.OpenTab openTab)
        {
            return JsonConvert.DeserializeObject<OpenTab>(openTab.Data);
        }
    }
}