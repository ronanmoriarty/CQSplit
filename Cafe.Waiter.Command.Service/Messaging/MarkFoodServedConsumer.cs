using System;
using System.Threading.Tasks;
using Cafe.Waiter.Contracts.Commands;
using log4net;
using MassTransit;

namespace Cafe.Waiter.Command.Service.Messaging
{
    public class MarkFoodServedConsumer : IConsumer<IMarkFoodServedCommand>
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(MarkFoodServedConsumer));

        public async Task Consume(ConsumeContext<IMarkFoodServedCommand> context)
        {
            var message = $"Received command: Type: {typeof(IOpenTabCommand).Name}; Command Id: {context.Message.Id}; Aggregate Id: {context.Message.AggregateId}";
            await Console.Out.WriteLineAsync(message);
            _logger.Debug(message);
        }
    }
}