using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Waiter.Queries.DAL.Models;
using Newtonsoft.Json;

namespace Cafe.Waiter.Queries.DAL.Repositories
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

        private IEnumerable<Serialized.OpenTab> GetAll()
        {
            return _waiterDbContext.OpenTabs.ToList();
        }

        public Serialized.OpenTab Get(Guid id)
        {
            return _waiterDbContext.OpenTabs.SingleOrDefault(x => x.Id == id);
        }

        public void Insert(OpenTab openTab)
        {
            var existingOpenTab = Get(openTab.Id);
            if (existingOpenTab == null)
            {
                _waiterDbContext.AddOpenTab(new Serialized.OpenTab
                {
                    Id = openTab.Id,
                    Data = JsonConvert.SerializeObject(openTab)
                });
            }
        }

        private OpenTab Map(Serialized.OpenTab openTab)
        {
            return JsonConvert.DeserializeObject<OpenTab>(openTab.Data);
        }
    }
}