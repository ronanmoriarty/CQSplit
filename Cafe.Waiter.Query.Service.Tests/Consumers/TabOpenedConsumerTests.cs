using System;
using System.Threading.Tasks;
using Cafe.Waiter.EventProjecting.Service.Consumers;
using Cafe.Waiter.EventProjecting.Service.Projectors;
using Cafe.Waiter.Events;
using MassTransit;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.EventProjecting.Service.Tests.Consumers
{
    [TestFixture]
    public class TabOpenedConsumerTests
    {
        private TabOpenedConsumer _tabOpenedConsumer;
        private readonly Guid _id = new Guid("74A17ECC-0408-4521-9284-71F4D8B460F0");

        [Test]
        public async Task Consumed_events_get_projected()
        {
            var projector = Substitute.For<ITabOpenedProjector>();
            _tabOpenedConsumer = new TabOpenedConsumer(projector);

            var @event = new TabOpened { Id = _id };
            var context = Substitute.For<ConsumeContext<TabOpened>>();
            context.Message.Returns(@event);

            await _tabOpenedConsumer.Consume(context);

            projector.Received(1).Project(@event);
        }
    }
}
