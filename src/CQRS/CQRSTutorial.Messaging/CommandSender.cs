using System.Threading.Tasks;
using CQRSTutorial.Core;
using NLog;

namespace CQRSTutorial.Messaging
{
    public class CommandSender : ICommandSender
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly ICommandSendConfiguration _commandSendConfiguration;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public CommandSender(ISendEndpointProvider sendEndpointProvider,
            ICommandSendConfiguration commandSendConfiguration)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _commandSendConfiguration = commandSendConfiguration;
        }

        public async Task Send<TCommand>(TCommand command)
            where TCommand : class, ICommand
        {
            // TODO: will write an end-to-end test around this shortly. Used to be Send(ICommand) but that just got sent to ICommand exchange which wasn't bound to anything, and so message was skipped (will undo then redo when writing test).
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(_commandSendConfiguration.QueueName);
            _logger.Debug($"Sending command {command.Id} for aggregate {command.AggregateId} to {_commandSendConfiguration.QueueName}");
            await sendEndpoint.Send(command);
        }
    }
}