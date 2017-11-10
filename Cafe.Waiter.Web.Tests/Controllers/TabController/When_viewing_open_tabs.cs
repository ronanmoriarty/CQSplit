using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;
using Cafe.Waiter.Web.Repositories;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests.Controllers.TabController
{
    [TestFixture]
    public class When_viewing_open_tabs
    {
        private Web.Controllers.TabController _tabController;
        private IOpenTabsRepository _openTabsRepository;
        private readonly Guid _id1 = new Guid("8580FF53-BC03-46A0-83FF-C71F35765BF1");
        private int _tableNumber1 = 123;
        private string _waiter1 = "Alice";
        private readonly Guid _id2 = new Guid("10D3745B-AB22-4C64-8CA1-ACDC51766C74");
        private int _tableNumber2 = 234;
        private string _waiter2 = "Bob";
        private List<OpenTab> _openTabs;

        [SetUp]
        public void SetUp()
        {
            _openTabsRepository = Substitute.For<IOpenTabsRepository>();
            _tabController = new Web.Controllers.TabController(null, _openTabsRepository, null, null);
            _openTabsRepository.GetOpenTabs().Returns(GetOpenTabs());

            _openTabs = _tabController.Get().ToList();
        }

        [Test]
        public void Gets_open_tabs_from_repository()
        {
            Assert.That(_openTabs.Count, Is.EqualTo(2));
            var firstOpenTab = _openTabs.First();
            Assert.That(firstOpenTab.Id, Is.EqualTo(_id1));
            Assert.That(firstOpenTab.Waiter, Is.EqualTo(_waiter1));
            Assert.That(firstOpenTab.TableNumber, Is.EqualTo(_tableNumber1));
            var secondOpenTab = _openTabs.Last();
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