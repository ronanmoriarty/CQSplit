using Microsoft.Extensions.Configuration;

namespace CQ.Messaging.RabbitMq
{
    public class RabbitMqHostConfiguration : IRabbitMqHostConfiguration
    {
        private readonly IConfigurationRoot _configurationRoot;

        public RabbitMqHostConfiguration(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        public string Uri => _configurationRoot["rabbitmq:uri"];
        public string Username => _configurationRoot["rabbitmq:username"];
        public string Password => _configurationRoot["rabbitmq:password"];
    }
}