using System;
using System.Threading.Tasks;
using Cafe.Waiter.Contracts;
using log4net;
using MassTransit;

namespace Cafe.Waiter.Service
{
    public class OpenTabCommandHandler : IConsumer<IOpenTab>
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(OpenTabCommandHandler));

        public async Task Consume(ConsumeContext<IOpenTab> context)
        {
            var message = $@"Received IOpenTab message: Id: {context.Message.Id}";
            await Console.Out.WriteLineAsync(message);
            _logger.Debug(message);
        }
    }
}