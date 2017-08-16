using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Cafe.Domain.Commands;
using Cafe.Waiter.Contracts;
using CQRSTutorial.Infrastructure;

namespace Cafe.Waiter.Web.Controllers
{
    public class TabController : Controller
    {
        private readonly IEndpointProvider _endpointProvider;

        public TabController(IEndpointProvider endpointProvider)
        {
            _endpointProvider = endpointProvider;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create()
        {
            var openTabCommand = CreateOpenTabCommand();
            var sendEndpoint = await _endpointProvider.GetSendEndpointFor<IOpenTab>();
            await sendEndpoint.Send(openTabCommand);
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