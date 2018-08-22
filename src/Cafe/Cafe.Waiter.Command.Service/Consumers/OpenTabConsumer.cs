using Cafe.Waiter.Contracts.Commands;
using CQSplit;
using CQSplit.Messaging;

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