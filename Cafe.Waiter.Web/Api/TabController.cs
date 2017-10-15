using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Cafe.Waiter.Commands;
using Cafe.Waiter.Contracts.Commands;
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
        private readonly IMenuRepository _menuRepository;

        public TabController(ITabDetailsRepository tabDetailsRepository,
            IOpenTabsRepository openTabsRepository,
            ICommandSender commandSender,
            IMenuRepository menuRepository)
        {
            _tabDetailsRepository = tabDetailsRepository;
            _openTabsRepository = openTabsRepository;
            _commandSender = commandSender;
            _menuRepository = menuRepository;
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
            _commandSender.Send(new PlaceOrderCommand
            {
                Id = Guid.NewGuid(),
                AggregateId = tabDetails.Id,
                Items = GetOrderedItems(tabDetails)
            });
        }

        private List<OrderedItem> GetOrderedItems(TabDetails tabDetails)
        {
            var menu = _menuRepository.GetMenu();
            return tabDetails.Items.Select(item => Map(item, menu)).ToList();
        }

        private OrderedItem Map(TabItem item, Menu menu)
        {
            var currentMenuItem = menu.Items.Single(menuItem => menuItem.Id == item.MenuNumber);
            return new OrderedItem
            {
                Description = item.Name,
                IsDrink = item.IsDrink,
                MenuNumber = item.MenuNumber,
                Price = currentMenuItem.Price
            };
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