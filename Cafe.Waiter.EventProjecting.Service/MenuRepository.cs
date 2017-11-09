using System.Linq;
using Cafe.Waiter.Queries.DAL;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;
using Newtonsoft.Json;

namespace Cafe.Waiter.EventProjecting.Service
{
    public class MenuRepository : IMenuRepository
    {
        private readonly IMenuConfiguration _menuConfiguration;
        private readonly IWaiterDbContext _waiterDbContext;

        public MenuRepository(IMenuConfiguration menuConfiguration, IWaiterDbContext waiterDbContext)
        {
            _menuConfiguration = menuConfiguration;
            _waiterDbContext = waiterDbContext;
        }

        public Menu GetMenu()
        {
            return Map(_waiterDbContext.Menus.Single(x => x.Id == _menuConfiguration.Id));
        }

        private Menu Map(Queries.DAL.Serialized.Menu serializedMenu)
        {
            return JsonConvert.DeserializeObject<Menu>(serializedMenu.Data);
        }
    }
}