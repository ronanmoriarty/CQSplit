using Cafe.Waiter.Contracts.Commands;
using CQ.Core;
using CQ.Messaging;

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