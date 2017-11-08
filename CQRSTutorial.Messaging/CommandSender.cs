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

        public async Task Send(ICommand command)
        {
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint();
            await sendEndpoint.Send(command);
        }
    }
}