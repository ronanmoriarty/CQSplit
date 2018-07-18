using Cafe.Waiter.Contracts.Commands;
using CQRSTutorial.Core;
using CQRSTutorial.Messaging;

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