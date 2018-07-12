using Cafe.Waiter.Contracts.Commands;
using CQRSTutorial.Core;
using CQRSTutorial.Messaging;

namespace Cafe.Waiter.Command.Service.Consumers
{
    public class MarkFoodServedConsumer : CommandConsumer<IMarkFoodServedCommand>
    {
        public MarkFoodServedConsumer(ICommandRouter commandRouter)
            : base(commandRouter)
        {
        }
    }
}