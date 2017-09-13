using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Cafe.Waiter.Queries.DAL;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests.TabController
{
    [TestFixture]
    public class When_viewing_open_tabs
    {
        private Controllers.TabController _tabController;
        private IOpenTabsRepository _openTabsRepository;
        private readonly Guid _id1 = new Guid("8580FF53-BC03-46A0-83FF-C71F35765BF1");
        private int _tableNumber1 = 123;
        private string _waiter1 = "Alice";
        private readonly Guid _id2 = new Guid("10D3745B-AB22-4C64-8CA1-ACDC51766C74");
        private int _tableNumber2 = 234;
        private string _waiter2 = "Bob";
        private ITabDetailsRepository _tabDetailsRepository;

        [SetUp]
        public void SetUp()
        {
            _openTabsRepository = Substitute.For<IOpenTabsRepository>();
            _tabDetailsRepository = Substitute.For<ITabDetailsRepository>();
            _tabController = new Controllers.TabController(null, _openTabsRepository, _tabDetailsRepository);
        }

        [Test]
        public void Gets_open_tabs_from_repository()
        {
            _openTabsRepository.GetOpenTabs().Returns(GetOpenTabs());

            var viewResult = (ViewResult)_tabController.Index().Result;

            Assert.That(viewResult.ViewName, Is.Empty); // ie. default view
            var openTabs = ((IEnumerable<OpenTab>)viewResult.Model).ToList();
            Assert.That(openTabs.Count, Is.EqualTo(2));
            var firstOpenTab = openTabs.First();
            Assert.That(firstOpenTab.Id, Is.EqualTo(_id1));
            Assert.That(firstOpenTab.Waiter, Is.EqualTo(_waiter1));
            Assert.That(firstOpenTab.TableNumber, Is.EqualTo(_tableNumber1));
            var secondOpenTab = openTabs.Last();
            Assert.That(secondOpenTab.Id, Is.EqualTo(_id2));
            Assert.That(secondOpenTab.Waiter, Is.EqualTo(_waiter2));
            Assert.That(secondOpenTab.TableNumber, Is.EqualTo(_tableNumber2));
        }

        private IEnumerable<OpenTab> GetOpenTabs()
        {
            return new List<OpenTab>
            {
                new OpenTab
                {
                    Id = _id1,
                    TableNumber = _tableNumber1,
                    Waiter = _waiter1
                },
                new OpenTab
                {
                    Id = _id2,
                    TableNumber = _tableNumber2,
                    Waiter = _waiter2
                }
            };
        }
    }
}