using log4net;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Infrastructure
{
    public class RabbitMqHostConfigurator : IRabbitMqHostConfigurator
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(RabbitMqMessageBusFactory));
        private readonly IRabbitMqHostConfiguration _rabbitMqHostConfiguration;

        public RabbitMqHostConfigurator(IRabbitMqHostConfiguration rabbitMqHostConfiguration)
        {
            _rabbitMqHostConfiguration = rabbitMqHostConfiguration;
        }

        public IRabbitMqHost Configure(IRabbitMqBusFactoryConfigurator sbc)
        {
            var hostAddress = _rabbitMqHostConfiguration.Uri;
            _logger.Debug($"Host address is: \"{hostAddress.AbsoluteUri}\"");
            var host = sbc.Host(hostAddress, h =>
            {
                h.Username(_rabbitMqHostConfiguration.Username);
                h.Password(_rabbitMqHostConfiguration.Password);
            });

            return host;
        }
    }
}