using CQ.Messaging;
using MassTransit;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.EventProjecting.Service.Tests
{
    [TestFixture]
    public class EventProjectingServiceTests
    {
        private EventProjectingService _eventProjectingService;
        private IMessageBusFactory _messageBusFactory;
        private IBusControl _busControl;

        [SetUp]
        public void SetUp()
        {
            _busControl = Substitute.For<IBusControl>();
            _messageBusFactory = Substitute.For<IMessageBusFactory>();
            _messageBusFactory.Create().Returns(_busControl);
            _eventProjectingService = new EventProjectingService(_messageBusFactory);
        }

        [Test]
        public void Starting_event_projecting_service_starts_the_bus()
        {
            _eventProjectingService.Start();

            _busControl.Received(1).Start();
        }

        [Test]
        public void Stopping_event_projecting_service_stops_the_bus()
        {
            _eventProjectingService.Start();
            _eventProjectingService.Stop();

            _busControl.Received(1).Stop();
        }
    }
}
