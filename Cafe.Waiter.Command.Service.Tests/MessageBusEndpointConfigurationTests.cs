using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Domain.Commands;
using Cafe.Waiter.Command.Service.Messaging;
using Cafe.Waiter.Contracts.Commands;
using CQRSTutorial.Infrastructure;
using MassTransit;
using NUnit.Framework;

namespace Cafe.Waiter.Command.Service.Tests
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
        public void All_commands_have_consumer_types_registered()
        {
            var allCommandTypes = GetAllCommandTypes();
            foreach (var commandType in allCommandTypes)
            {
                var contractInterface = GetContractInterface(commandType);
                Assert.That(_messageBusEndpointConfiguration.ReceiveEndpoints.Any(
                    receiveEndpointMapping => HasConsumerDefinedForCommand(receiveEndpointMapping, contractInterface)));
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

        private static Type GetContractInterface(Type commandType)
        {
            var contractsNamespace = typeof(IOpenTabCommand).Namespace;
            Console.WriteLine($"Getting interface in {contractsNamespace} namespace for {commandType.Name}");
            return commandType
                .GetInterfaces()
                .Single(interfaceType => interfaceType.Namespace == contractsNamespace);
        }

        private static IEnumerable<Type> GetAllCommandTypes()
        {
            return typeof(OpenTabCommand)
                .Assembly
                .GetTypes()
                .Where(type => type.Namespace == typeof(OpenTabCommand).Namespace);
        }
    }
}
