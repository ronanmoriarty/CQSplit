using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Cafe.Waiter.Commands;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;
using CQRSTutorial.Messaging;

namespace Cafe.Waiter.Web.Controllers
{
    public class TabController : Controller
    {
        private readonly ICommandSender _commandSender;
        private readonly IOpenTabsRepository _openTabsRepository;

        public TabController(ICommandSender commandSender,
            IOpenTabsRepository openTabsRepository)
        {
            _commandSender = commandSender;
            _openTabsRepository = openTabsRepository;
        }

        public async Task<ActionResult> Index()
        {
            return View(_openTabsRepository.GetOpenTabs());
        }

        [HttpPost]
        public async Task<ActionResult> Create()
        {
            var openTabCommand = CreateOpenTabCommand();
            await _commandSender.Send(openTabCommand);
            return RedirectToAction("Index", new { tabId = openTabCommand.AggregateId });
        }

        private OpenTabCommand CreateOpenTabCommand()
        {
            return new OpenTabCommand
            {
                Id = Guid.NewGuid(),
                AggregateId = Guid.NewGuid(),
                Waiter = "John",
                TableNumber = 5
            };
        }

        public async Task<ActionResult> Details(Guid tabId)
        {
            return View();
        }

        [HttpPost]
        public void Details(TabDetails tabDetails)
        {
        }
    }
}