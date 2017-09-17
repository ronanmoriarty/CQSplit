using System.Web.Mvc;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cafe.Waiter.Web.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenuRepository _menuRepository;

        public MenuController(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public ContentResult Index()
        {
            var menu = _menuRepository.GetMenu();
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var json = JsonConvert.SerializeObject(menu, Formatting.Indented, jsonSerializerSettings);

            return Content(json, "application/json");
        }
    }
}