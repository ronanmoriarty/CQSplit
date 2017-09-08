using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Waiter.Command.Service.Consumers;
using CQRSTutorial.Infrastructure;

namespace Cafe.Waiter.Command.Service
{
    public class MessageBusEndpointConfiguration : IMessageBusEndpointConfiguration
    {
        public MessageBusEndpointConfiguration(ReceiveEndpointMappingFactory receiveEndpointMappingFactory)
        {
            ReceiveEndpoints = GetConsumerTypes().Select(x => receiveEndpointMappingFactory.CreateMappingFor(x, "consumer", "command")).ToList();
        }

        private static List<Type> GetConsumerTypes()
        {
            var consumerTypes = new List<Type>();
            consumerTypes.AddRange(new[]
            {
                typeof(OpenTabConsumer),
                typeof(PlaceOrderConsumer),
                typeof(MarkDrinksServedConsumer),
                typeof(MarkFoodServedConsumer),
                typeof(CloseTabConsumer)
            });
            return consumerTypes;
        }

        public IEnumerable<ReceiveEndpointMapping> ReceiveEndpoints { get; }
    }
}