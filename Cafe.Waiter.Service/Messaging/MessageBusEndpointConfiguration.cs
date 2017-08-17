using System.Collections.Generic;
using CQRSTutorial.Infrastructure;

namespace Cafe.Waiter.Service.Messaging
{
    public class MessageBusEndpointConfiguration : IMessageBusEndpointConfiguration
    {
        public MessageBusEndpointConfiguration()
        {
            ReceiveEndpoints = new List<ReceiveEndpointMapping>
            {
                new ReceiveEndpointMapping(null, typeof(OpenTabCommandHandler)),
                new ReceiveEndpointMapping(null, typeof(CloseTabCommandHandler)),
                new ReceiveEndpointMapping(null, typeof(MarkDrinksServedCommandHandler)),
                new ReceiveEndpointMapping(null, typeof(MarkFoodServedCommandHandler)),
                new ReceiveEndpointMapping(null, typeof(PlaceOrderCommandHandler)),
            };
        }

        public IEnumerable<ReceiveEndpointMapping> ReceiveEndpoints { get; }
    }
}