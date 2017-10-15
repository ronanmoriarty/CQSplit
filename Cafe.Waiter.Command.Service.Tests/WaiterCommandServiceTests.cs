using CQRSTutorial.Messaging;
using MassTransit;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.Command.Service.Tests
{
    [TestFixture]
    public class WaiterCommandServiceTests
    {
        private WaiterCommandService _waiterCommandService;
        private IMessageBusFactory _messageBusFactory;
        private IBusControl _busControl;

        [SetUp]
        public void SetUp()
        {
            _busControl = Substitute.For<IBusControl>();
            _messageBusFactory = Substitute.For<IMessageBusFactory>();
            _messageBusFactory.Create().Returns(_busControl);
            _waiterCommandService = new WaiterCommandService(_messageBusFactory);
        }

        [Test]
        public void Starting_waiter_service_starts_the_bus()
        {
            _waiterCommandService.Start();

            _busControl.Received(1).Start();
        }

        [Test]
        public void Stopping_waiter_service_stops_the_bus()
        {
            _waiterCommandService.Start();
            _waiterCommandService.Stop();

            _busControl.Received(1).Stop();
        }
    }
}
