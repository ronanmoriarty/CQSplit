using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Waiter.Contracts.Queries;
using Cafe.Waiter.Query.Service.Messaging;
using Cafe.Waiter.Query.Service.Queries;
using CQRSTutorial.Infrastructure;
using MassTransit;
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
            _messageBusEndpointConfiguration = new MessageBusEndpointConfiguration();
        }

        [Test]
        public void All_queries_have_consumer_types_registered()
        {
            var allQueryTypes = GetAllQueryTypes();
            foreach (var queryType in allQueryTypes)
            {
                var contractInterface = GetContractInterface(queryType);
                Assert.That(_messageBusEndpointConfiguration.ReceiveEndpoints.Any(receiveEndpointMapping => HasConsumerDefinedForCommand(receiveEndpointMapping, contractInterface)));
            }
        }

        private bool HasConsumerDefinedForCommand(ReceiveEndpointMapping receiveEndpointMapping, Type contractInterface)
        {
            Console.WriteLine($"Looking for receive-endpoint-mapping that has consumer type that implements {typeof(IConsumer<>).Name}<{contractInterface.Name}>...");
            return receiveEndpointMapping.ConsumerType.GetInterfaces()
                .Any(interfaceType => interfaceType.IsGenericType
                                      && interfaceType.GetGenericTypeDefinition() == typeof(IConsumer<>)
                                      && interfaceType.GenericTypeArguments.Single() == contractInterface);
        }

        private static Type GetContractInterface(Type queryType)
        {
            var contractsNamespace = typeof(IOpenTabsQuery).Namespace;
            Console.WriteLine($"Getting interface in {contractsNamespace} namespace for {queryType.Name}");
            return queryType
                .GetInterfaces()
                .Single(interfaceType => interfaceType.Namespace == contractsNamespace);
        }

        private static IEnumerable<Type> GetAllQueryTypes()
        {
            return typeof(OpenTabsQuery)
                .Assembly
                .GetTypes()
                .Where(type => type.Namespace == typeof(OpenTabsQuery).Namespace);
        }
    }
}