using Cafe.Waiter.Contracts.Commands;
using CQRSTutorial.Core;

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