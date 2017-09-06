using System;
using System.Collections.Generic;
using System.Linq;
using CQRSTutorial.Infrastructure;
using Cafe.Waiter.Query.Service.Consumers;

namespace Cafe.Waiter.Query.Service
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
            //consumerTypes.AddRange(new[]
            //{
            //    typeof(TabOpenedConsumer)
            //}); // TODO will uncomment when I've got the tests in place
            return consumerTypes;
        }

        public IEnumerable<ReceiveEndpointMapping> ReceiveEndpoints { get; }
    }
}