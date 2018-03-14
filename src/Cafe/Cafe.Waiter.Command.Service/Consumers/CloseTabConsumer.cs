using Cafe.Waiter.Contracts.Commands;
using CQRSTutorial.Core;

namespace Cafe.Waiter.Command.Service.Consumers
{
    public class CloseTabConsumer : Consumer<ICloseTabCommand>
    {
        public CloseTabConsumer(ICommandDispatcher commandDispatcher)
            : base(commandDispatcher)
        {
        }
    }
}