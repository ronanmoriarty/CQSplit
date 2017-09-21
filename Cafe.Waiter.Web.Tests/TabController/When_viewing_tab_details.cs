using System;
using System.Web.Mvc;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests.TabController
{
    [TestFixture]
    public class When_viewing_tab_details
    {
        private Controllers.TabController _tabController;

        [SetUp]
        public void SetUp()
        {
            _tabController = new Controllers.TabController(null, null);
        }

        [Test]
        public void Gets_tab_details_from_repository()
        {
            var viewResult = (ViewResult)_tabController.Details();

            Assert.That(viewResult.ViewName, Is.Empty); // ie. default view
        }
    }
}