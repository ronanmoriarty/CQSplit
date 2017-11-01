using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Waiter.EventProjecting.Service.Consumers;
using CQRSTutorial.Messaging;

namespace Cafe.Waiter.EventProjecting.Service
{
    public class MessageBusEndpointConfiguration : IMessageBusEndpointConfiguration
    {
        public MessageBusEndpointConfiguration(ReceiveEndpointMappingFactory receiveEndpointMappingFactory)
        {
            ReceiveEndpoints = GetConsumerTypes().Select(x => receiveEndpointMappingFactory.CreateMappingFor(x, "consumer", "event")).ToList();
        }

        private static List<Type> GetConsumerTypes()
        {
            var consumerTypes = new List<Type>();
            consumerTypes.AddRange(new[]
            {
                typeof(TabOpenedConsumer)
            });

            return consumerTypes;
        }

        public IEnumerable<ReceiveEndpointMapping> ReceiveEndpoints { get; }
    }
}