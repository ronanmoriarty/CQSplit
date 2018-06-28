using Cafe.Waiter.Contracts.Commands;
using CQRSTutorial.Core;

namespace Cafe.Waiter.Command.Service.Consumers
{
    public class OpenTabConsumer : Consumer<IOpenTabCommand>
    {
        public OpenTabConsumer(ICommandRouter commandRouter)
            : base(commandRouter)
        {
        }
    }
}