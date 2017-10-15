using System;
using System.Web.Mvc;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;
using CQRSTutorial.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cafe.Waiter.Web.Api
{
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

        public ContentResult TabDetails(Guid tabId)
        {
            var tabDetails = _tabDetailsRepository.GetTabDetails(tabId);
            return CreateContentResult(tabDetails);
        }

        public ContentResult Index()
        {
            var openTabs = _openTabsRepository.GetOpenTabs();
            return CreateContentResult(openTabs);
        }

        [HttpPost]
        public void PlaceOrder(TabDetails tabDetails)
        {
            _commandSender.Send(_placeOrderCommandFactory.Create(tabDetails));
        }

        private ContentResult CreateContentResult(object obj)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented, jsonSerializerSettings);

            return Content(json, "application/json");
        }
    }
}