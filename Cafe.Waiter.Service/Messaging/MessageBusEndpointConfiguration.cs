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
                // TODO Will have a thnk about what kind of tests I want around these mappings. Not sure at this stage.
                new ReceiveEndpointMapping("open_tab", typeof(OpenTabConsumer)),
                new ReceiveEndpointMapping("close_tab", typeof(CloseTabConsumer)),
                new ReceiveEndpointMapping("mark_drinks_served", typeof(MarkDrinksServedConsumer)),
                new ReceiveEndpointMapping("mark_food_served", typeof(MarkFoodServedConsumer)),
                new ReceiveEndpointMapping("place_order", typeof(PlaceOrderConsumer)),
            };
        }

        public IEnumerable<ReceiveEndpointMapping> ReceiveEndpoints { get; }
    }
}