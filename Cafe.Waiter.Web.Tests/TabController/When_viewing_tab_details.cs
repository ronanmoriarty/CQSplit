using System;
using System.Web.Mvc;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests.TabController
{
    [TestFixture]
    public class When_viewing_tab_details
    {
        private Controllers.TabController _tabController;
        private readonly Guid _id1 = new Guid("8580FF53-BC03-46A0-83FF-C71F35765BF1");

        [SetUp]
        public void SetUp()
        {
            _tabController = new Controllers.TabController(null, null);
        }

        [Test]
        public void Gets_tab_details_from_repository()
        {
            var viewResult = (ViewResult)_tabController.Details(_id1).Result;

            Assert.That(viewResult.ViewName, Is.Empty); // ie. default view
        }
    }
}