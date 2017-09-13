using System.Threading.Tasks;
using CQRSTutorial.Core;
using log4net;
using MassTransit;

namespace Cafe.Waiter.Command.Service.Consumers
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

            _commandDispatcher.Dispatch(context.Message);
        }
    }
}