using Cafe.Waiter.Contracts.Commands;
using CQRSTutorial.Core;
using CQRSTutorial.Messaging;

namespace Cafe.Waiter.Command.Service.Consumers
{
    public class PlaceOrderConsumer : Consumer<IPlaceOrderCommand>
    {
        public PlaceOrderConsumer(ICommandRouter commandRouter)
            : base(commandRouter)
        {
        }
    }
}