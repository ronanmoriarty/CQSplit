using System;
using Cafe.Domain.Events;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.Repositories;
using Cafe.Waiter.Query.Service.Projectors;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.Query.Service.Tests.Projectors
{
    [TestFixture]
    public class TabOpenedProjectorTests
    {
        private TabOpenedProjector _tabOpenedProjector;
        private readonly Guid _id = new Guid("6E7B25E5-5B4F-4C08-9147-8DAF69E3FCE2");
        private readonly int _tableNumber = 654;
        private string _waiter = "Jim";
        private IOpenTabsRepository _openTabsRepository;

        [SetUp]
        public void SetUp()
        {
            _openTabsRepository = Substitute.For<IOpenTabsRepository>();
            _tabOpenedProjector = new TabOpenedProjector(_openTabsRepository);
        }

        [Test]
        public void Projects_event_to_OpenTab_ReadModel()
        {
            WhenTabOpenedEventReceived();

            AssertThatOpenTabInserted();
        }

        private void WhenTabOpenedEventReceived()
        {
            _tabOpenedProjector.Project(CreateTabOpenedEvent());
        }

        private TabOpened CreateTabOpenedEvent()
        {
            return new TabOpened
            {
                Id = _id,
                TableNumber = _tableNumber,
                Waiter = _waiter
            };
        }

        private void AssertThatOpenTabInserted()
        {
            _openTabsRepository.Received(1).Insert(Arg.Is<OpenTab>(x => x.Id == _id));
        }
    }
}
