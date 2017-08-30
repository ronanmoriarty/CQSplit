using System;
using System.Threading.Tasks;
using Cafe.Waiter.Contracts.Commands;
using log4net;
using MassTransit;

namespace Cafe.Waiter.Service.Messaging
{
    public class MarkDrinksServedConsumer : IConsumer<IMarkDrinksServedCommand>
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(MarkDrinksServedConsumer));

        public async Task Consume(ConsumeContext<IMarkDrinksServedCommand> context)
        {
            var message = $"Received command: Type: {typeof(IOpenTabCommand).Name}; Command Id: {context.Message.Id}; Aggregate Id: {context.Message.AggregateId}";
            await Console.Out.WriteLineAsync(message);
            _logger.Debug(message);
        }
    }
}