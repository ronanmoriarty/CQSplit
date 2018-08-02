using System.Linq;
using Cafe.DAL.Common;
using Cafe.Waiter.Queries.DAL.Models;
using Newtonsoft.Json;

namespace Cafe.Waiter.Web.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly IMenuConfiguration _menuConfiguration;
        private readonly IConnectionStringProvider _connectionStringProvider;

        public MenuRepository(IMenuConfiguration menuConfiguration, IConnectionStringProvider connectionStringProvider)
        {
            _menuConfiguration = menuConfiguration;
            _connectionStringProvider = connectionStringProvider;
        }

        public Menu GetMenu()
        {
            return Map(new WaiterDbContext(_connectionStringProvider.GetConnectionString()).Menus.Single(x => x.Id == _menuConfiguration.Id));
        }

        private Menu Map(Queries.DAL.Serialized.Menu serializedMenu)
        {
            return JsonConvert.DeserializeObject<Menu>(serializedMenu.Data);
        }
    }
}