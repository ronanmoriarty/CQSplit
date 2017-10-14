using System;
using System.Web.Mvc;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests.Api.TabController
{
    [TestFixture]
    public class When_getting_tab_details
    {
        private ITabDetailsRepository _tabDetailsRepository;
        private Web.Api.TabController _tabController;
        private readonly Guid _id = new Guid("965F6FAC-2265-4F0B-8F6D-3974CD4D9900");
        private TabDetails _tabDetails;
        private string _contentType;

        [SetUp]
        public void SetUp()
        {
            _tabDetailsRepository = Substitute.For<ITabDetailsRepository>();
            _tabDetailsRepository.GetTabDetails(_id).Returns(new TabDetails { Id = _id });
            _tabController = new Web.Api.TabController(_tabDetailsRepository, null);

            WhenGettingTabDetails();
        }

        [Test]
        public void Gets_details_from_repository()
        {
            Assert.That(_tabDetails.Id, Is.EqualTo(_id));
        }

        [Test]
        public void Content_type_is_json()
        {
            Assert.That(_contentType, Is.EqualTo("application/json"));
        }

        private void WhenGettingTabDetails()
        {
            var contentResult = _tabController.TabDetails(_id);
            _tabDetails = GetTabDetailsFromContent(contentResult);
            _contentType = contentResult.ContentType;
        }

        private static TabDetails GetTabDetailsFromContent(ContentResult contentResult)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.DeserializeObject<TabDetails>(contentResult.Content, jsonSerializerSettings);
        }
    }
}
