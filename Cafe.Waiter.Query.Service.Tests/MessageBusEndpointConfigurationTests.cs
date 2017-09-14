using System;
using System.Linq;
using Cafe.Waiter.Query.Service.Consumers;
using CQRSTutorial.Messaging;
using NUnit.Framework;

namespace Cafe.Waiter.Query.Service.Tests
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

        [TestCase("tab_opened_event", typeof(TabOpenedConsumer))]
        public void All_events_have_consumer_types_registered(string expectedServiceAddress, Type expectedConsumerType)
        {
                Assert.That(_messageBusEndpointConfiguration.ReceiveEndpoints.Any(
                    receiveEndpointMapping => receiveEndpointMapping.ConsumerType == expectedConsumerType
                                              && receiveEndpointMapping.ServiceAddress == expectedServiceAddress));
        }
    }
}
