using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Cafe.Waiter.Queries.DAL.Models;
using CQRSTutorial.DAL;
using Newtonsoft.Json;

namespace Cafe.Waiter.Queries.DAL.Repositories
{
    public class OpenTabsRepository : IOpenTabsRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public OpenTabsRepository(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public IEnumerable<OpenTab> GetOpenTabs()
        {
            return GetAll().Select(Map);
        }

        private IEnumerable<Serialized.OpenTab> GetAll()
        {
            return new WaiterDbContext().OpenTabs.ToList();
        }

        public Serialized.OpenTab Get(Guid id)
        {
            return new WaiterDbContext().OpenTabs.SingleOrDefault(x => x.Id == id);
        }

        public void Insert(OpenTab openTab)
        {
            var existingOpenTab = Get(openTab.Id);
            if (existingOpenTab == null)
            {
                var waiterDbContext = new WaiterDbContext();
                waiterDbContext.OpenTabs.Add(new Serialized.OpenTab
                {
                    Id = openTab.Id,
                    Data = JsonConvert.SerializeObject(openTab)
                });
                waiterDbContext.SaveChanges();
            }
        }

        private OpenTab Map(Serialized.OpenTab openTab)
        {
            return JsonConvert.DeserializeObject<OpenTab>(openTab.Data);
        }
    }
}