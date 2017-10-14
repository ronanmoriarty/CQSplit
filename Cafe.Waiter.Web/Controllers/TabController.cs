using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Cafe.Waiter.Commands;
using Cafe.Waiter.Web.Models;
using CQRSTutorial.Messaging;

namespace Cafe.Waiter.Web.Controllers
{
    public class TabController : Controller
    {
        private readonly ICommandSender _commandSender;

        public TabController(ICommandSender commandSender)
        {
            _commandSender = commandSender;
        }

        public ViewResult Index()
        {
            return View();
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