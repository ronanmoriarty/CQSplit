using Cafe.Waiter.Contracts.Commands;
using CQSplit;
using CQSplit.Messaging;

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