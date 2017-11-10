using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cafe.Waiter.Commands;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Web.Models;
using Cafe.Waiter.Web.Repositories;
using CQRSTutorial.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace Cafe.Waiter.Web.Controllers
{
    [Route("api/[controller]")]
    public class TabController : Controller
    {
        private readonly ITabDetailsRepository _tabDetailsRepository;
        private readonly IOpenTabsRepository _openTabsRepository;
        private readonly ICommandSender _commandSender;
        private readonly IPlaceOrderCommandFactory _placeOrderCommandFactory;

        public TabController(ITabDetailsRepository tabDetailsRepository,
            IOpenTabsRepository openTabsRepository,
            ICommandSender commandSender,
            IPlaceOrderCommandFactory placeOrderCommandFactory)
        {
            _tabDetailsRepository = tabDetailsRepository;
            _openTabsRepository = openTabsRepository;
            _commandSender = commandSender;
            _placeOrderCommandFactory = placeOrderCommandFactory;
        }

        // GET api/tab/5
        [HttpGet("{tabId}")]
        public TabDetails Get(Guid tabId)
        {
            return _tabDetailsRepository.GetTabDetails(tabId);
        }

        // GET api/tab
        [HttpGet]
        public IEnumerable<OpenTab> Get()
        {
            return _openTabsRepository.GetOpenTabs();
        }

        [HttpPost]
        public void PlaceOrder(TabDetails tabDetails)
        {
            _commandSender.Send(_placeOrderCommandFactory.Create(tabDetails));
        }

        [HttpPost]
        public async Task Create(CreateTabModel model)
        {
            var openTabCommand = CreateOpenTabCommand(model);
            await _commandSender.Send(openTabCommand);
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