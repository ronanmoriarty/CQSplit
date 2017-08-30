using Cafe.Waiter.Contracts.Commands;
using CQRSTutorial.Core;

namespace Cafe.Waiter.Service.Messaging
{
    public class OpenTabConsumer : Consumer<IOpenTabCommand>
    {
        public OpenTabConsumer(ICommandDispatcher commandDispatcher)
            : base(commandDispatcher)
        {
        }
    }
}