using System.Collections.Generic;
using CQRSTutorial.Messaging;

namespace Cafe.Waiter.Web.Messaging
{
    // TODO remove this class - see note in Cafe.Waiter.Web.Messaging.ConsumerFactory
    public class RabbitMqMessageBusEndpointConfiguration : IMessageBusEndpointConfiguration
    {
        public RabbitMqMessageBusEndpointConfiguration()
        {
            ReceiveEndpoints = new List<ReceiveEndpointMapping>();
        }

        public IEnumerable<ReceiveEndpointMapping> ReceiveEndpoints { get; }
    }
}