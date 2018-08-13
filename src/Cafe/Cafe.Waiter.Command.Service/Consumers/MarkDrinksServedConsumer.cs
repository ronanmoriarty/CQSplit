using Cafe.Waiter.Contracts.Commands;
using CQSplit.Core;
using CQSplit.Messaging;

namespace Cafe.Waiter.Command.Service.Consumers
{
    public class MarkDrinksServedConsumer : CommandConsumer<IMarkDrinksServedCommand>
    {
        public MarkDrinksServedConsumer(ICommandRouter commandRouter)
            : base(commandRouter)
        {
        }
    }
}