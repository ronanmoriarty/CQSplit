using System;
using System.Linq;
using System.Threading.Tasks;
using Cafe.DAL.Tests.Common;
using Cafe.Waiter.EventProjecting.Service.Consumers;
using Cafe.Waiter.EventProjecting.Service.DAL;
using Cafe.Waiter.Events;
using Cafe.Waiter.Queries.DAL;
using Cafe.Waiter.Queries.DAL.Models;
using MassTransit;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.EventProjecting.Service.Tests.Consumers
{
    [TestFixture, Category(TestConstants.Integration)]
    public class TabOpenedConsumerTests
    {
        private TabOpenedEventConsumer _tabOpenedEventConsumer;
        private readonly Guid _id = new Guid("6E7B25E5-5B4F-4C08-9147-8DAF69E3FCE2");
        private readonly Guid _aggregateId = new Guid("C32030D7-C783-4EF9-88F7-1CEEED79A5E0");
        private readonly int _tableNumber = 654;
        private readonly string _waiter = "Jim";
        private readonly SqlExecutor _sqlExecutor = new SqlExecutor(ReadModelConnectionStringProvider.ConnectionString);

        [SetUp]
        public void SetUp()
        {
            _sqlExecutor.ExecuteNonQuery($"DELETE FROM dbo.OpenTabs WHERE Id = '{_aggregateId}'");
            _tabOpenedEventConsumer = Container.Instance.Resolve<TabOpenedEventConsumer>();
        }

        [Test]
        public async Task TabOpened_event_projected_to_an_OpenTab()
        {
            await WhenTabOpenedEventReceived();

            AssertThatOpenTabInsertedWithStatusInitiallySetToSeated();
        }

        [Test]
        public async Task Same_event_can_be_received_more_than_once()
        {
            await WhenTabOpenedEventReceived();
            await WhenTabOpenedEventReceived();

            AssertThatOpenTabInsertedWithStatusInitiallySetToSeated();
        }

        private async Task WhenTabOpenedEventReceived()
        {
            var eventMessage = Substitute.For<ConsumeContext<TabOpened>>();
            eventMessage.Message.Returns(CreateTabOpenedEvent());
            await _tabOpenedEventConsumer.Consume(eventMessage);
        }

        private TabOpened CreateTabOpenedEvent()
        {
            return new TabOpened
            {
                Id = _id,
                AggregateId = _aggregateId,
                TableNumber = _tableNumber,
                Waiter = _waiter
            };
        }

        private void AssertThatOpenTabInsertedWithStatusInitiallySetToSeated()
        {
            var serializedOpenTab = new WaiterDbContext(ReadModelConnectionStringProvider.ConnectionString).OpenTabs.Single(x => x.Id == _aggregateId);
            var retrievedOpenTab = JsonConvert.DeserializeObject<OpenTab>(serializedOpenTab.Data);
            Assert.That(retrievedOpenTab.Id, Is.EqualTo(_aggregateId));
            Assert.That(retrievedOpenTab.TableNumber, Is.EqualTo(_tableNumber));
            Assert.That(retrievedOpenTab.Waiter, Is.EqualTo(_waiter));
            Assert.That(retrievedOpenTab.Status, Is.EqualTo(TabStatus.Seated));
        }
    }
}
