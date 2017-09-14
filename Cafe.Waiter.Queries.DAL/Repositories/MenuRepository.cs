using System;
using Cafe.Waiter.Queries.DAL.Models;
using CQRSTutorial.DAL;
using Newtonsoft.Json;
using NHibernate;

namespace Cafe.Waiter.Queries.DAL.Repositories
{
    public class MenuRepository : RepositoryBase<Serialized.Menu>, IMenuRepository
    {
        public MenuRepository(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
        }

        public Menu GetMenu(Guid id)
        {
            var serializedMenu = Get(id);
            return Map(serializedMenu);
        }

        private Menu Map(Serialized.Menu serializedMenu)
        {
            return JsonConvert.DeserializeObject<Menu>(serializedMenu.Data);
        }
    }
}