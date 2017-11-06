using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Waiter.EventProjecting.Service.Consumers;
using CQRSTutorial.Messaging;
using NUnit.Framework;

namespace Cafe.Waiter.EventProjecting.Service.Tests
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

        [Test]
        public void All_events_have_consumer_types_registered()
        {
            var typesDerivingFromConsumer = GetTypesDerivingFromConsumer();
            foreach (var consumerType in typesDerivingFromConsumer)
            {
                Console.WriteLine($"Checking consumer type {consumerType.FullName}");
                Assert.That(_messageBusEndpointConfiguration.GetConsumerTypes().Any(type => type == consumerType));
            }
        }

        private static IEnumerable<Type> GetTypesDerivingFromConsumer()
        {
            return typeof(TabOpenedConsumer).Assembly.GetTypes().Where(type =>
            {
                var baseType = type.BaseType;
                return baseType != null
                    && baseType.IsGenericType
                    && baseType.GetGenericTypeDefinition() == typeof(Consumer<>);
            });
        }
    }
}
