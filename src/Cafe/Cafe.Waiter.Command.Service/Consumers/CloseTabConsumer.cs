using Cafe.Waiter.Contracts.Commands;
using CQSplit.Core;
using CQSplit.Messaging;

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