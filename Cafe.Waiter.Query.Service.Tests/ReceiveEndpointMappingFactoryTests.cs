using System;
using NUnit.Framework;
using Cafe.Waiter.Query.Service.Consumers;
using CQRSTutorial.Messaging;

namespace Cafe.Waiter.Query.Service.Tests
{
    [TestFixture]
    public class ReceiveEndpointMappingFactoryTests
    {
        [TestCase(typeof(TabOpenedConsumer), "tab_opened_event")]
        public void Creates_service_address_replacing_consumer_with_command(Type consumerType, string expectedServiceAddress)
        {
            var mapping = new ReceiveEndpointMappingFactory(new ServiceAddressProvider()).CreateMappingFor(consumerType, "consumer", "event");

            Assert.That(mapping.ServiceAddress, Is.EqualTo(expectedServiceAddress));
        }
    }
}