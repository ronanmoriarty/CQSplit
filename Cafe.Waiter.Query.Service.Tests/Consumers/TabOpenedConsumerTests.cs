using NUnit.Framework;
using NSubstitute;
using Cafe.Waiter.Query.Service.Consumers;
using MassTransit;
using Cafe.Domain.Events;
using System;
using System.Threading.Tasks;

namespace Cafe.Waiter.Query.Service.Tests.Consumers
{
    [TestFixture]
    public class TabOpenedConsumerTests
    {
        private TabOpenedConsumer _tabOpenedConsumer;
        private Guid _id = new Guid("74A17ECC-0408-4521-9284-71F4D8B460F0");

        [Test]
        public async Task Consumed_events_get_projected()
        {
            var projector = Substitute.For<IProjector>();
            _tabOpenedConsumer = new TabOpenedConsumer(projector);

            var @event = new TabOpened { Id = _id };
            var context = Substitute.For<ConsumeContext<TabOpened>>();
            context.Message.Returns(@event);

            await _tabOpenedConsumer.Consume(context);

            projector.Received(1).Project(@event);
        }
    }
}
