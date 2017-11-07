using System;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests.Controllers.TabController
{
    [TestFixture]
    public class When_getting_tab_details
    {
        private ITabDetailsRepository _tabDetailsRepository;
        private Web.Controllers.TabController _tabController;
        private readonly Guid _id = new Guid("965F6FAC-2265-4F0B-8F6D-3974CD4D9900");
        private TabDetails _tabDetails;

        [SetUp]
        public void SetUp()
        {
            _tabDetailsRepository = Substitute.For<ITabDetailsRepository>();
            _tabDetailsRepository.GetTabDetails(_id).Returns(new TabDetails { Id = _id });
            _tabController = new Web.Controllers.TabController(_tabDetailsRepository, null, null, null);

            WhenGettingTabDetails();
        }

        [Test]
        public void Gets_details_from_repository()
        {
            Assert.That(_tabDetails.Id, Is.EqualTo(_id));
        }

        private void WhenGettingTabDetails()
        {
            _tabDetails = _tabController.Get(_id);
        }
    }
}
