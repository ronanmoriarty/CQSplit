using System;
using System.Linq;
using Cafe.Waiter.Service.Messaging;
using CQRSTutorial.Infrastructure;
using NUnit.Framework;

namespace Cafe.Waiter.Service.Tests
{
    [TestFixture]
    public class MessageBusEndpointConfigurationTests
    {
        private MessageBusEndpointConfiguration _messageBusEndpointConfiguration;

        [SetUp]
        public void SetUp()
        {
            _messageBusEndpointConfiguration = new MessageBusEndpointConfiguration(new ReceiveEndpointMappingFactory(new ServiceAddressProvider()));
        }

        [TestCase("open_tab_command", typeof(OpenTabConsumer))]
        [TestCase("place_order_command", typeof(PlaceOrderConsumer))]
        [TestCase("mark_food_served_command", typeof(MarkFoodServedConsumer))]
        [TestCase("mark_drinks_served_command", typeof(MarkDrinksServedConsumer))]
        [TestCase("close_tab_command", typeof(CloseTabConsumer))]
        public void All_commands_have_consumer_types_registered(string expectedServiceAddress, Type expectedConsumerType)
        {
                Assert.That(_messageBusEndpointConfiguration.ReceiveEndpoints.Any(
                    receiveEndpointMapping => receiveEndpointMapping.ConsumerType == expectedConsumerType
                                              && receiveEndpointMapping.ServiceAddress == expectedServiceAddress));
        }
    }
}
