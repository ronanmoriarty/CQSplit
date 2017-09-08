using System;
using Cafe.Waiter.Command.Service.Consumers;
using CQRSTutorial.Infrastructure;
using NUnit.Framework;

namespace Cafe.Waiter.Command.Service.Tests
{
    [TestFixture]
    public class ReceiveEndpointMappingFactoryTests
    {
        [TestCase(typeof(OpenTabConsumer), "open_tab_command")]
        [TestCase(typeof(PlaceOrderConsumer), "place_order_command")]
        [TestCase(typeof(MarkFoodServedConsumer), "mark_food_served_command")]
        [TestCase(typeof(MarkDrinksServedConsumer), "mark_drinks_served_command")]
        [TestCase(typeof(CloseTabConsumer), "close_tab_command")]
        public void Creates_service_address_replacing_consumer_with_command(Type consumerType, string expectedServiceAddress)
        {
            var mapping = new ReceiveEndpointMappingFactory(new ServiceAddressProvider()).CreateMappingFor(consumerType, "consumer", "command");

            Assert.That(mapping.ServiceAddress, Is.EqualTo(expectedServiceAddress));
        }
    }
}