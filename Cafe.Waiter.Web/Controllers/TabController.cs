using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Cafe.Domain.Commands;
using Cafe.Waiter.Contracts.Commands;
using Cafe.Waiter.DAL;
using CQRSTutorial.Infrastructure;

namespace Cafe.Waiter.Web.Controllers
{
    public class TabController : Controller
    {
        private readonly IEndpointProvider _endpointProvider;
        private readonly IOpenTabsRepository _openTabsRepository;

        public TabController(IEndpointProvider endpointProvider, IOpenTabsRepository openTabsRepository)
        {
            _endpointProvider = endpointProvider;
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
            var sendEndpoint = await _endpointProvider.GetSendEndpointFor<IOpenTabCommand>();
            await sendEndpoint.Send(openTabCommand);
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
    }
}