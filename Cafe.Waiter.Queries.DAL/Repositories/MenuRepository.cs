using Cafe.Waiter.Queries.DAL.Models;
using CQRSTutorial.DAL;
using Newtonsoft.Json;
using NHibernate;

namespace Cafe.Waiter.Queries.DAL.Repositories
{
    public class MenuRepository : RepositoryBase<Serialized.Menu>, IMenuRepository
    {
        private readonly IMenuConfiguration _menuConfiguration;

        public MenuRepository(ISessionFactory sessionFactory,
            IMenuConfiguration menuConfiguration)
            : base(sessionFactory)
        {
            _menuConfiguration = menuConfiguration;
        }

        public Menu GetMenu()
        {
            var serializedMenu = Get(_menuConfiguration.Id);
            return Map(serializedMenu);
        }

        private Menu Map(Serialized.Menu serializedMenu)
        {
            return JsonConvert.DeserializeObject<Menu>(serializedMenu.Data);
        }
    }
}