using System.Collections.Generic;
using CQRSTutorial.Infrastructure;

namespace Cafe.Waiter.Service.Messaging
{
    public class RabbitMqMessageBusEndpointConfiguration : IMessageBusEndpointConfiguration
    {
        public RabbitMqMessageBusEndpointConfiguration()
        {
            ReceiveEndpoints = new List<ReceiveEndpointMapping> { new ReceiveEndpointMapping("open_tab", typeof(OpenTabCommandHandler)) };
        }

        public IEnumerable<ReceiveEndpointMapping> ReceiveEndpoints { get; }
    }
}