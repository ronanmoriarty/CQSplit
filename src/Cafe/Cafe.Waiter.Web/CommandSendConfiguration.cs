using CQSplit.Messaging;
using Microsoft.Extensions.Configuration;

namespace Cafe.Waiter.Web
{
    public class CommandSendConfiguration : ICommandSendConfiguration
    {
        private readonly IConfigurationRoot _configuration;

        public CommandSendConfiguration(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public string QueueName => _configuration["CommandServiceQueueName"];
    }
}
