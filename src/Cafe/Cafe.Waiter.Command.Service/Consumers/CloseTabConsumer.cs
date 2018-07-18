using Cafe.Waiter.Contracts.Commands;
using CQRSTutorial.Core;
using CQRSTutorial.Messaging;

namespace Cafe.Waiter.Command.Service.Consumers
{
    public class CloseTabConsumer : CommandConsumer<ICloseTabCommand>
    {
        public CloseTabConsumer(ICommandRouter commandRouter)
            : base(commandRouter)
        {
        }
    }
}