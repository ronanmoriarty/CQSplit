using Cafe.Waiter.Contracts.Commands;
using CQ.Core;
using CQ.Messaging;

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