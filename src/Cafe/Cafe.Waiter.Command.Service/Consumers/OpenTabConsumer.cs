using Cafe.Waiter.Contracts.Commands;
using CQ.Core;
using CQ.Messaging;

namespace Cafe.Waiter.Command.Service.Consumers
{
    public class OpenTabConsumer : CommandConsumer<IOpenTabCommand>
    {
        public OpenTabConsumer(ICommandRouter commandRouter)
            : base(commandRouter)
        {
        }
    }
}