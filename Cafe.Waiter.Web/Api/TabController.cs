using System;
using System.Web.Mvc;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cafe.Waiter.Web.Api
{
    public class TabController : Controller
    {
        private readonly ITabDetailsRepository _tabDetailsRepository;
        private readonly IOpenTabsRepository _openTabsRepository;

        public TabController(ITabDetailsRepository tabDetailsRepository, IOpenTabsRepository openTabsRepository)
        {
            _tabDetailsRepository = tabDetailsRepository;
            _openTabsRepository = openTabsRepository;
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

        private ContentResult CreateContentResult(object obj)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented, jsonSerializerSettings);

            return Content(json, "application/json");
        }

        [HttpPost]
        public void Details(TabDetails tabDetails)
        {
        }
    }
}