using System;
using Cafe.Domain.Events;
using Cafe.Waiter.Queries.DAL;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.NHibernate;
using Cafe.Waiter.Queries.DAL.Repositories;
using Cafe.Waiter.Query.Service.Projectors;
using CQRSTutorial.DAL.Tests.Common;
using Newtonsoft.Json;
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
        private OpenTabsRepository _openTabsRepository;
        private readonly SqlExecutor _sqlExecutor = new SqlExecutor(ReadModelConnectionStringProviderFactory.Instance);

        [SetUp]
        public void SetUp()
        {
            _sqlExecutor.ExecuteNonQuery($"DELETE FROM dbo.OpenTabs WHERE Id = '{_id}'");
            _openTabsRepository = new OpenTabsRepository(ReadModelSessionFactory.Instance);
            _tabOpenedProjector = Container.Instance.Resolve<TabOpenedProjector>();
        }

        [Test]
        public void Projects_event_to_OpenTab_ReadModel()
        {
            WhenTabOpenedEventReceived();

            AssertThatOpenTabInserted();
        }

        [Test]
        public void Projecting_an_event_that_already_exists_does_not_cause_a_primary_key_violation()
        {
            WhenTabOpenedEventReceived();
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
            var serializedOpenTab = _openTabsRepository.Get(_id);
            var retrievedOpenTab = JsonConvert.DeserializeObject<OpenTab>(serializedOpenTab.Data);
            Assert.That(retrievedOpenTab.Id, Is.EqualTo(_id));
            Assert.That(retrievedOpenTab.TableNumber, Is.EqualTo(_tableNumber));
            Assert.That(retrievedOpenTab.Waiter, Is.EqualTo(_waiter));
        }
    }
}
