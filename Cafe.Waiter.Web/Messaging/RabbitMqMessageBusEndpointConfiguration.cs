using System.Collections.Generic;
using CQRSTutorial.Infrastructure;

namespace Cafe.Waiter.Web.Messaging
{
    public class RabbitMqMessageBusEndpointConfiguration : IMessageBusEndpointConfiguration
    {
        public RabbitMqMessageBusEndpointConfiguration()
        {
            ReceiveEndpoints = new List<ReceiveEndpointMapping>();
        }

        public IEnumerable<ReceiveEndpointMapping> ReceiveEndpoints { get; }
    }
}