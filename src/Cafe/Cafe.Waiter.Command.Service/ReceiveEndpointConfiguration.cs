using CQ.Messaging;
using Microsoft.Extensions.Configuration;

namespace Cafe.Waiter.Command.Service
{
    public class ReceiveEndpointConfiguration : IReceiveEndpointConfiguration
    {
        private readonly IConfigurationRoot _configuration;

        public ReceiveEndpointConfiguration()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json")
                .Build();
        }

        public string QueueName => _configuration["QueueName"];
    }
}
