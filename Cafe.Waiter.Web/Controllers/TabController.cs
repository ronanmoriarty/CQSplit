using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Cafe.Waiter.Commands;
using Cafe.Waiter.Contracts.Commands;
using Cafe.Waiter.Queries.DAL.Repositories;
using CQRSTutorial.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cafe.Waiter.Web.Controllers
{
    public class TabController : Controller
    {
        private readonly IEndpointProvider _endpointProvider;
        private readonly IOpenTabsRepository _openTabsRepository;
        private readonly ITabDetailsRepository _tabDetailsRepository;

        public TabController(IEndpointProvider endpointProvider,
            IOpenTabsRepository openTabsRepository,
            ITabDetailsRepository tabDetailsRepository)
        {
            _endpointProvider = endpointProvider;
            _openTabsRepository = openTabsRepository;
            _tabDetailsRepository = tabDetailsRepository;
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

        public async Task<ActionResult> Details(Guid tabId)
        {
            var tabDetails = _tabDetailsRepository.GetTabDetails(tabId);
            return View(tabDetails);
        }

        public ContentResult TabDetails(Guid tabId)
        {
            var tabDetails = _tabDetailsRepository.GetTabDetails(tabId);
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var json = JsonConvert.SerializeObject(tabDetails, Formatting.Indented, jsonSerializerSettings);

            return Content(json, "application/json");
        }
    }
}