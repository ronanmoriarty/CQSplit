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

        public TabController(ITabDetailsRepository tabDetailsRepository)
        {
            _tabDetailsRepository = tabDetailsRepository;
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

        [HttpPost]
        public void Details(TabDetails tabDetails)
        {
        }
    }
}