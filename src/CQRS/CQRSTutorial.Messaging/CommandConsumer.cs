using System.Threading.Tasks;
using CQRSTutorial.Core;
using log4net;
using MassTransit;

namespace CQRSTutorial.Messaging
{
    public abstract class CommandConsumer<TCommand> : IConsumer<TCommand>
        where TCommand : class, ICommand
    {
        private readonly ICommandRouter _commandRouter;
        private readonly ILog _logger = LogManager.GetLogger(typeof(CommandConsumer<>));

        protected CommandConsumer(ICommandRouter commandRouter)
        {
            _commandRouter = commandRouter;
        }

        public async Task Consume(ConsumeContext<TCommand> context)
        {
            var text = $"Received command: {GetMessageDescription(context)}";
            _logger.Debug(text);

            try
            {
                _commandRouter.Route(context.Message);
            }
            catch (System.Exception exception)
            {
                _logger.Error($"Error processing {GetMessageDescription(context)}");
                _logger.Error(exception);
                throw;
            }
        }

        private string GetMessageDescription(ConsumeContext<TCommand> context)
        {
            return $"Type: {typeof(TCommand).Name}; Command Id: {context.Message.Id}; Aggregate Id: {context.Message.AggregateId}";
        }
    }
}
