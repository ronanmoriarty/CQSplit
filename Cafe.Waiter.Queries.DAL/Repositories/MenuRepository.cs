using System.Linq;
using Cafe.Waiter.Queries.DAL.Models;
using Newtonsoft.Json;

namespace Cafe.Waiter.Queries.DAL.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly IMenuConfiguration _menuConfiguration;

        public MenuRepository(IMenuConfiguration menuConfiguration)
        {
            _menuConfiguration = menuConfiguration;
        }

        public Menu GetMenu()
        {
            return Map(new WaiterDbContext(ReadModelConnectionStringProviderFactory.Instance.GetConnectionStringProvider().GetConnectionString()).Menus.Single(x => x.Id == _menuConfiguration.Id));
        }

        private Menu Map(Serialized.Menu serializedMenu)
        {
            return JsonConvert.DeserializeObject<Menu>(serializedMenu.Data);
        }
    }
}