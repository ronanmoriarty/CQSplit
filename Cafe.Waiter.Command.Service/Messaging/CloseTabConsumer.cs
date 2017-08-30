using System;
using System.Threading.Tasks;
using Cafe.Waiter.Contracts.Commands;
using log4net;
using MassTransit;

namespace Cafe.Waiter.Command.Service.Messaging
{
    public class CloseTabConsumer : IConsumer<ICloseTabCommand>
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(CloseTabConsumer));

        public async Task Consume(ConsumeContext<ICloseTabCommand> context)
        {
            var message = $"Received command: Type: {typeof(IOpenTabCommand).Name}; Command Id: {context.Message.Id}; Aggregate Id: {context.Message.AggregateId}";
            await Console.Out.WriteLineAsync(message);
            _logger.Debug(message);
        }
    }
}