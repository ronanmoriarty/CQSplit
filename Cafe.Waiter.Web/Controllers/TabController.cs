using System;
using System.Web.Mvc;
using Cafe.Domain.Commands;

namespace Cafe.Waiter.Web.Controllers
{
    public class TabController : Controller
    {
        private readonly IMessageBus _messageBus;

        public TabController(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }

        // GET: Tab
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create()
        {
            var openTabCommand = CreateOpenTabCommand();
            _messageBus.Send(openTabCommand);
            return RedirectToAction("Index", new {tabId = openTabCommand.AggregateId});
        }

        private OpenTab CreateOpenTabCommand()
        {
            return new OpenTab
            {
                Id = Guid.NewGuid(),
                AggregateId = Guid.NewGuid(),
                Waiter = "John",
                TableNumber = 5
            };
        }
    }
}