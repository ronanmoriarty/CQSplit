using Cafe.Waiter.Contracts.Commands;
using CQRSTutorial.Core;

namespace Cafe.Waiter.Command.Service.Messaging
{
    public class OpenTabConsumer : Consumer<IOpenTab>
    {
        public OpenTabConsumer(ICommandDispatcher commandDispatcher)
            : base(commandDispatcher)
        {
        }
    }
}