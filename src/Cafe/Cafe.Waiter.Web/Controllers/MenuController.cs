using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Cafe.Waiter.Web.Controllers
{
    [Route("api/[controller]")]
    public class MenuController : Controller
    {
        private readonly IMenuRepository _menuRepository;

        public MenuController(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        [HttpGet]
        public Menu Get()
        {
            return _menuRepository.GetMenu();
        }
    }
}