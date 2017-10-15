using System.Threading.Tasks;
using CQRSTutorial.Core;

namespace CQRSTutorial.Messaging
{
    public class CommandSender : ICommandSender
    {
        private readonly IEndpointProvider _endpointProvider;

        public CommandSender(IEndpointProvider endpointProvider)
        {
            _endpointProvider = endpointProvider;
        }

        public async Task Send(ICommand command)
        {
            var sendEndpoint = await _endpointProvider.GetSendEndpointFor(command.GetType());
            await sendEndpoint.Send(command);
        }
    }
}