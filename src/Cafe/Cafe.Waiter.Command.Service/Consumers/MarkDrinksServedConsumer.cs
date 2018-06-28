using Cafe.Waiter.Contracts.Commands;
using CQRSTutorial.Core;

namespace Cafe.Waiter.Command.Service.Consumers
{
    public class MarkDrinksServedConsumer : Consumer<IMarkDrinksServedCommand>
    {
        public MarkDrinksServedConsumer(ICommandRouter commandRouter)
            : base(commandRouter)
        {
        }
    }
}