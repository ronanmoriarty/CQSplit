using System;
using Microsoft.Extensions.Configuration;

namespace CQSplit.Messaging.RabbitMq.Tests
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
        public int RetryLimit => Convert.ToInt32(_configurationRoot["rabbitmq:retryLimit"]);
        public int DelayInSecondsBetweenRetries => Convert.ToInt32(_configurationRoot["rabbitmq:delayInSecondsBetweenRetries"]);
    }
}