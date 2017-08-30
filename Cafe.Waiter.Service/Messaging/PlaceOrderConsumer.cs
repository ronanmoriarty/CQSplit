using System;
using System.Threading.Tasks;
using Cafe.Waiter.Contracts.Commands;
using log4net;
using MassTransit;

namespace Cafe.Waiter.Service.Messaging
{
    public class PlaceOrderConsumer : IConsumer<IPlaceOrderCommand>
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(PlaceOrderConsumer));

        public async Task Consume(ConsumeContext<IPlaceOrderCommand> context)
        {
            var message = $"Received command: Type: {typeof(IOpenTabCommand).Name}; Command Id: {context.Message.Id}; Aggregate Id: {context.Message.AggregateId}";
            await Console.Out.WriteLineAsync(message);
            _logger.Debug(message);
        }
    }
}