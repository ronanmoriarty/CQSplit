using System;
using System.Threading.Tasks;
using Cafe.Waiter.Contracts;
using log4net;
using MassTransit;

namespace Cafe.Waiter.Service.Messaging
{
    public class CloseTabConsumer : IConsumer<ICloseTab>
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(CloseTabConsumer));

        public async Task Consume(ConsumeContext<ICloseTab> context)
        {
            var message = $"Received command: Type: {typeof(IOpenTab).Name}; Command Id: {context.Message.Id}; Aggregate Id: {context.Message.AggregateId}";
            await Console.Out.WriteLineAsync(message);
            _logger.Debug(message);
        }
    }
}