using System;
using System.Web.Mvc;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;
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
        private JsonResult _jsonResult;

        [SetUp]
        public void SetUp()
        {
            _tabDetailsRepository = Substitute.For<ITabDetailsRepository>();
            _tabDetailsRepository.GetTabDetails(_id).Returns(new TabDetails { Id = _id });
            _tabController = new Web.Api.TabController(_tabDetailsRepository, null, null, null);

            WhenGettingTabDetails();
        }

        [Test]
        public void Gets_details_from_repository()
        {
            Assert.That(_tabDetails.Id, Is.EqualTo(_id));
        }

        [Test]
        public void AllowGet()
        {
            Assert.That(_jsonResult.JsonRequestBehavior, Is.EqualTo(JsonRequestBehavior.AllowGet));
        }

        private void WhenGettingTabDetails()
        {
            _jsonResult = _tabController.TabDetails(_id);
            _tabDetails = (TabDetails)_jsonResult.Data;
        }
    }
}
