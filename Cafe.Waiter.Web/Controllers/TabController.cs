using System.Web.Mvc;

namespace Cafe.Waiter.Web.Controllers
{
    public class TabController : Controller
    {
        // GET: Tab
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create()
        {
            return Redirect("Index");
        }
    }
}