using System.Threading.Tasks;
using CQRSTutorial.Core;
using log4net;
using MassTransit;

namespace Cafe.Waiter.Service.Messaging
{
    public abstract class Consumer<TCommand> : IConsumer<TCommand>
        where TCommand : class, ICommand
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ILog _logger = LogManager.GetLogger(typeof(Consumer<>));

        protected Consumer(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public async Task Consume(ConsumeContext<TCommand> context)
        {
            var text = $"Received command: Type: {typeof(TCommand).Name}; Command Id: {context.Message.Id}; Aggregate Id: {context.Message.AggregateId}";
            _logger.Debug(text);

            // TODO: can't see how to instantiate a ConsumeContext for unit testing. Will continue to spike this for now.
            _commandDispatcher.Dispatch(context.Message);
        }
    }
}