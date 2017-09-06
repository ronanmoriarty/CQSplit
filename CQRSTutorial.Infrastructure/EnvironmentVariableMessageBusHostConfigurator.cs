using System;
using log4net;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Infrastructure
{
    public class EnvironmentVariableMessageBusHostConfigurator : IMessageBusHostConfigurator
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(RabbitMqMessageBusFactory));

        public Uri Uri => new Uri(Environment.GetEnvironmentVariable("RABBITMQ_URI"));
        public string Username => Environment.GetEnvironmentVariable("RABBITMQ_USERNAME");
        public string Password => Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");

        public IRabbitMqHost Configure(IRabbitMqBusFactoryConfigurator sbc)
        {
            var hostAddress = Uri;
            _logger.Debug($"Host address is: \"{hostAddress.AbsoluteUri}\"");
            var host = sbc.Host(hostAddress, h =>
            {
                h.Username(Username);
                h.Password(Password);
            });

            return host;
        }
    }
}