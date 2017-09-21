using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Cafe.Waiter.Commands;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;
using Cafe.Waiter.Web.Models;
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

        public ViewResult Index()
        {
            return View(_openTabsRepository.GetOpenTabs());
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateTabModel model)
        {
            var openTabCommand = CreateOpenTabCommand(model);
            await _commandSender.Send(openTabCommand);
            return RedirectToAction("Index", new { tabId = openTabCommand.AggregateId });
        }

        public ViewResult Details()
        {
            return View();
        }

        [HttpPost]
        public void Details(TabDetails tabDetails)
        {
        }

        private OpenTabCommand CreateOpenTabCommand(CreateTabModel model)
        {
            return new OpenTabCommand
            {
                Id = Guid.NewGuid(),
                AggregateId = Guid.NewGuid(),
                Waiter = model.Waiter,
                TableNumber = model.TableNumber
            };
        }
    }
}