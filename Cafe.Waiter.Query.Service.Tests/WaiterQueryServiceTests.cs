using CQRSTutorial.Messaging;
using MassTransit;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.EventProjecting.Service.Tests
{
    [TestFixture]
    public class WaiterQueryServiceTests
    {
        private WaiterQueryService _waiterQueryService;
        private IMessageBusFactory _messageBusFactory;
        private IBusControl _busControl;

        [SetUp]
        public void SetUp()
        {
            _busControl = Substitute.For<IBusControl>();
            _messageBusFactory = Substitute.For<IMessageBusFactory>();
            _messageBusFactory.Create().Returns(_busControl);
            _waiterQueryService = new WaiterQueryService(_messageBusFactory);
        }

        [Test]
        public void Starting_waiter_service_starts_the_bus()
        {
            _waiterQueryService.Start();

            _busControl.Received(1).Start();
        }

        [Test]
        public void Stopping_waiter_service_stops_the_bus()
        {
            _waiterQueryService.Start();
            _waiterQueryService.Stop();

            _busControl.Received(1).Stop();
        }
    }
}
