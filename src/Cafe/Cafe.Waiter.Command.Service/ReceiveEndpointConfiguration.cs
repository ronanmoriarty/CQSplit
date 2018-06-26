using CQRSTutorial.Messaging;

namespace Cafe.Waiter.Command.Service
{
    public class ReceiveEndpointConfiguration : IReceiveEndpointConfiguration
    {
        public string QueueName { get; } = "cafe.waiter.command.service"; // TODO: get this from config again
    }
}
