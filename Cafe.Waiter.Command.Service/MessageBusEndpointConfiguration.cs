using System;
using System.Collections.Generic;
using Cafe.Waiter.Command.Service.Consumers;
using CQRSTutorial.Messaging;

namespace Cafe.Waiter.Command.Service
{
    public class MessageBusEndpointConfiguration : IMessageBusEndpointConfiguration
    {
        public MessageBusEndpointConfiguration(ReceiveEndpointMappingFactory receiveEndpointMappingFactory)
        {
        }

        public List<Type> GetConsumerTypes()
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
    }
}