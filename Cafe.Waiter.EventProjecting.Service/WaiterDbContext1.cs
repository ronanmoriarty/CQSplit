using System.Linq;
using CQRSTutorial.DAL.Common;

namespace Cafe.Waiter.EventProjecting.Service
{
    public class WaiterDbContext1 : IWaiterDbContext
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public WaiterDbContext1(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public IQueryable<Queries.DAL.Serialized.Menu> Menus => new WaiterDbContext(_connectionStringProvider.GetConnectionString()).Menus;
    }
}