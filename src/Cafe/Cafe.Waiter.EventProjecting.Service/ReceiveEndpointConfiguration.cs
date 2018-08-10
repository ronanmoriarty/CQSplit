using CQ.Messaging;
using Microsoft.Extensions.Configuration;

namespace Cafe.Waiter.EventProjecting.Service
{
    public class ReceiveEndpointConfiguration : IReceiveEndpointConfiguration
    {
        private const bool Optional = true;
        private readonly IConfigurationRoot _configurationRoot;

        public ReceiveEndpointConfiguration()
        {
            _configurationRoot = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json")
                .Build();
        }

        public string QueueName => _configurationRoot["QueueName"];
    }
}