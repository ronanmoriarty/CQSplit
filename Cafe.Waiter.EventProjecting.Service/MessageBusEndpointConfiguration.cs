using System;
using System.Collections.Generic;
using Cafe.Waiter.EventProjecting.Service.Consumers;
using CQRSTutorial.Messaging;

namespace Cafe.Waiter.EventProjecting.Service
{
    public class MessageBusEndpointConfiguration : IMessageBusEndpointConfiguration
    {
        public List<Type> GetConsumerTypes()
        {
            var consumerTypes = new List<Type>();
            consumerTypes.AddRange(new[]
            {
                typeof(TabOpenedConsumer)
            });

            return consumerTypes;
        }
    }
}