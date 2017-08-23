using System.Collections.Generic;
using CQRSTutorial.Infrastructure;

namespace Cafe.Waiter.Query.Service.Messaging
{
    public class MessageBusEndpointConfiguration : IMessageBusEndpointConfiguration
    {
        public MessageBusEndpointConfiguration()
        {
            ReceiveEndpoints = new List<ReceiveEndpointMapping>();
        }

        public IEnumerable<ReceiveEndpointMapping> ReceiveEndpoints { get; }
    }
}