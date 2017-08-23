using System;
using System.Threading.Tasks;
using Cafe.Waiter.Contracts;
using Cafe.Waiter.Contracts.Commands;
using log4net;
using MassTransit;

namespace Cafe.Waiter.Service.Messaging
{
    public class MarkDrinksServedConsumer : IConsumer<IMarkDrinksServed>
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(MarkDrinksServedConsumer));

        public async Task Consume(ConsumeContext<IMarkDrinksServed> context)
        {
            var message = $"Received command: Type: {typeof(IOpenTab).Name}; Command Id: {context.Message.Id}; Aggregate Id: {context.Message.AggregateId}";
            await Console.Out.WriteLineAsync(message);
            _logger.Debug(message);
        }
    }
}