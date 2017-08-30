using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Cafe.Domain.Commands;
using Cafe.Waiter.Contracts.Commands;
using Cafe.Waiter.Contracts.Queries;
using Cafe.Waiter.Contracts.QueryResponses;
using CQRSTutorial.Infrastructure;
using MassTransit;

namespace Cafe.Waiter.Web.Controllers
{
    public class TabController : Controller
    {
        private readonly IEndpointProvider _endpointProvider;
        private readonly IRequestClient<IOpenTabsQuery, IOpenTabsQueryResponse> _requestClient;

        public TabController(IEndpointProvider endpointProvider, IRequestClient<IOpenTabsQuery, IOpenTabsQueryResponse> requestClient)
        {
            _endpointProvider = endpointProvider;
            _requestClient = requestClient;
        }

        public async Task<ActionResult> Index()
        {
            var openTabsQuery = new OpenTabsQuery { Id = Guid.NewGuid() };
            var openTabsQueryResponse = await _requestClient.Request(openTabsQuery);
            return View(openTabsQueryResponse);
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