using System.Threading.Tasks;
using CQRSTutorial.Core;

namespace CQRSTutorial.Messaging
{
    public class CommandSender : ICommandSender
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public CommandSender(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task Send<TCommand>(TCommand command, string queueName)
            where TCommand : class, ICommand
        {
            // TODO: will write an end-to-end test around this shortly. Used to be Send(ICommand) but that just got sent to ICommand exchange which wasn't bound to anything, and so message was skipped (will undo then redo when writing test).
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(queueName);
            await sendEndpoint.Send(command);
        }
    }
}