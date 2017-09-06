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
                new ReceiveEndpointMapping("open_tab_command", typeof(OpenTabConsumer)),
                new ReceiveEndpointMapping("close_tab_command", typeof(CloseTabConsumer)),
                new ReceiveEndpointMapping("mark_drinks_served_command", typeof(MarkDrinksServedConsumer)),
                new ReceiveEndpointMapping("mark_food_served_command", typeof(MarkFoodServedConsumer)),
                new ReceiveEndpointMapping("place_order_command", typeof(PlaceOrderConsumer)),
            };
        }

        public IEnumerable<ReceiveEndpointMapping> ReceiveEndpoints { get; }
    }
}