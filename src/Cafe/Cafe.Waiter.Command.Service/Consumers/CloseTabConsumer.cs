using Cafe.Waiter.Contracts.Commands;
using CQ.Core;
using CQ.Messaging;

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