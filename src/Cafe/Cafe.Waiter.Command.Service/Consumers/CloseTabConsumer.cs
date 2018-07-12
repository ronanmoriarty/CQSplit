using Cafe.Waiter.Contracts.Commands;
using CQRSTutorial.Core;
using CQRSTutorial.Messaging;

namespace Cafe.Waiter.Command.Service.Consumers
{
    public class CloseTabConsumer : Consumer<ICloseTabCommand>
    {
        public CloseTabConsumer(ICommandRouter commandRouter)
            : base(commandRouter)
        {
        }
    }
}