using System;
using System.Threading.Tasks;
using Cafe.Waiter.Events;
using Cafe.Waiter.Queries.DAL;
using Cafe.Waiter.Queries.DAL.Models;
using Cafe.Waiter.Queries.DAL.NHibernate;
using Cafe.Waiter.Queries.DAL.Repositories;
using Cafe.Waiter.Query.Service.Consumers;
using CQRSTutorial.DAL.Tests.Common;
using MassTransit;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.Query.Service.Tests.Projectors
{
    [TestFixture]
    public class TabOpenedConsumerTests
    {
        private TabOpenedConsumer _tabOpenedConsumer;
        private readonly Guid _id = new Guid("6E7B25E5-5B4F-4C08-9147-8DAF69E3FCE2");
        private readonly int _tableNumber = 654;
        private readonly string _waiter = "Jim";
        private OpenTabsRepository _openTabsRepository;
        private readonly SqlExecutor _sqlExecutor = new SqlExecutor(ReadModelConnectionStringProviderFactory.Instance);

        [SetUp]
        public void SetUp()
        {
            _sqlExecutor.ExecuteNonQuery($"DELETE FROM dbo.OpenTabs WHERE Id = '{_id}'");
            _openTabsRepository = new OpenTabsRepository(ReadModelSessionFactory.Instance);
            _tabOpenedConsumer = Container.Instance.Resolve<TabOpenedConsumer>();
        }

        [Test]
        public async Task TabOpened_event_projected_to_an_OpenTab()
        {
            await WhenTabOpenedEventReceived();

            AssertThatOpenTabInserted();
        }

        [Test]
        public async Task Same_event_can_be_received_more_than_once()
        {
            await WhenTabOpenedEventReceived();
            await WhenTabOpenedEventReceived();

            AssertThatOpenTabInserted();
        }

        private async Task WhenTabOpenedEventReceived()
        {
            var eventMessage = Substitute.For<ConsumeContext<TabOpened>>();
            eventMessage.Message.Returns(CreateTabOpenedEvent());
            await _tabOpenedConsumer.Consume(eventMessage);
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
