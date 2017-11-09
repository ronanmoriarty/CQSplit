using System.Linq;
using Cafe.Waiter.Queries.DAL;
using Cafe.Waiter.Queries.DAL.Serialized;
using CQRSTutorial.DAL.Common;

namespace Cafe.Waiter.EventProjecting.Service
{
    public class WaiterDbContextAdapter : IWaiterDbContext
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public WaiterDbContextAdapter(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public IQueryable<Menu> Menus => GetWaiterDbContext().Menus;

        public IQueryable<OpenTab> OpenTabs => GetWaiterDbContext().OpenTabs;

        public IQueryable<TabDetails> TabDetails => GetWaiterDbContext().TabDetails;

        public void AddOpenTab(OpenTab openTab)
        {
            var waiterDbContext = GetWaiterDbContext();
            waiterDbContext.OpenTabs.Add(openTab);
            waiterDbContext.SaveChanges();
        }

        private WaiterDbContext GetWaiterDbContext()
        {
            return new WaiterDbContext(_connectionStringProvider.GetConnectionString());
        }
    }
}