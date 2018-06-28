using Cafe.Waiter.Contracts.Commands;
using CQRSTutorial.Core;

namespace Cafe.Waiter.Command.Service.Consumers
{
    public class MarkFoodServedConsumer : Consumer<IMarkFoodServedCommand>
    {
        public MarkFoodServedConsumer(ICommandRouter commandRouter)
            : base(commandRouter)
        {
        }
    }
}