using System;
using System.Web.Mvc;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests.TabController
{
    [TestFixture]
    public class When_viewing_tab_details
    {
        private Controllers.TabController _tabController;
        private IOpenTabsRepository _openTabsRepository;
        private ITabDetailsRepository _tabDetailsRepository;
        private readonly Guid _id1 = new Guid("8580FF53-BC03-46A0-83FF-C71F35765BF1");
        private TabDetails _tabDetails;
        private IMenuRepository _menuRepository;

        [SetUp]
        public void SetUp()
        {
            _openTabsRepository = Substitute.For<IOpenTabsRepository>();
            _tabDetailsRepository = Substitute.For<ITabDetailsRepository>();
            _menuRepository = Substitute.For<IMenuRepository>();
            _tabController = new Controllers.TabController(null, _openTabsRepository, _tabDetailsRepository, _menuRepository);
            _tabDetails = GetTabDetails();
        }

        [Test]
        public void Gets_tab_details_from_repository()
        {
            _tabDetailsRepository.GetTabDetails(_id1).Returns(_tabDetails);

            var viewResult = (ViewResult)_tabController.Details(_id1).Result;

            Assert.That(viewResult.ViewName, Is.Empty); // ie. default view
            var retrievedTabDetails = (TabDetails)viewResult.Model;
            Assert.That(retrievedTabDetails, Is.EqualTo(_tabDetails));
        }

        private TabDetails GetTabDetails()
        {
            return new TabDetails
            {
                Id = _id1
            };
        }
    }
}