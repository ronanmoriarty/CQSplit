using System;
using System.Threading.Tasks;
using Cafe.Waiter.Contracts;
using CQRSTutorial.Core;
using log4net;
using MassTransit;

namespace Cafe.Waiter.Service.Messaging
{
    public class OpenTabConsumer : IConsumer<IOpenTab>
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ILog _logger = LogManager.GetLogger(typeof(OpenTabConsumer));

        public OpenTabConsumer(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public async Task Consume(ConsumeContext<IOpenTab> context)
        {
            var text = $"Received command: Type: {typeof(IOpenTab).Name}; Command Id: {context.Message.Id}; Aggregate Id: {context.Message.AggregateId}";
            await Console.Out.WriteLineAsync(text);
            _logger.Debug(text);

            // TODO: can't see how to instantiate a ConsumeContext for unit testing. Will continue to spike this for now.
            _commandDispatcher.Dispatch(context.Message);
        }
    }
}