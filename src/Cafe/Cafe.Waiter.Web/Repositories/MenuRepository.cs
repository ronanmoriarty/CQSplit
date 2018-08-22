using System.Linq;
using Cafe.Waiter.Queries.DAL.Models;
using Newtonsoft.Json;

namespace Cafe.Waiter.Web.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly IMenuConfiguration _menuConfiguration;
        private readonly string _connectionString;

        public MenuRepository(IMenuConfiguration menuConfiguration, string connectionString)
        {
            _menuConfiguration = menuConfiguration;
            _connectionString = connectionString;
        }

        public Menu GetMenu()
        {
            return Map(new WaiterDbContext(_connectionString).Menus.Single(x => x.Id == _menuConfiguration.Id));
        }

        private Menu Map(Queries.DAL.Serialized.Menu serializedMenu)
        {
            return JsonConvert.DeserializeObject<Menu>(serializedMenu.Data);
        }
    }
}