using System;
using System.Threading.Tasks;
using Cafe.Waiter.Contracts;
using log4net;
using MassTransit;

namespace Cafe.Waiter.Service.Messaging
{
    public class MarkFoodServedConsumer : IConsumer<IMarkFoodServed>
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(MarkFoodServedConsumer));

        public async Task Consume(ConsumeContext<IMarkFoodServed> context)
        {
            var message = $"Received command: Type: {typeof(IOpenTab).Name}; Command Id: {context.Message.Id}; Aggregate Id: {context.Message.AggregateId}";
            await Console.Out.WriteLineAsync(message);
            _logger.Debug(message);
        }
    }
}