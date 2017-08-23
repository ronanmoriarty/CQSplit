using Cafe.Waiter.Contracts;
using CQRSTutorial.Core;

namespace Cafe.Waiter.Service.Messaging
{
    public class OpenTabConsumer : Consumer<IOpenTab>
    {
        public OpenTabConsumer(ICommandDispatcher commandDispatcher)
            : base(commandDispatcher)
        {
        }
    }
}