using log4net;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Infrastructure
{
    public class RabbitMqMessageBusFactory : IMessageBusFactory
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(RabbitMqMessageBusFactory));
        private readonly IRabbitMQEndpointConfigurator _rabbitMqEndpointConfigurator;
        private readonly IRabbitMqHostConfiguration _rabbitMqHostConfiguration;

        public RabbitMqMessageBusFactory(
            IRabbitMqHostConfiguration rabbitMqHostConfiguration,
            IRabbitMQEndpointConfigurator rabbitMqEndpointConfigurator)
        {
            _rabbitMqHostConfiguration = rabbitMqHostConfiguration;
            _rabbitMqEndpointConfigurator = rabbitMqEndpointConfigurator;
        }

        public IBusControl Create()
        {
            return Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = Configure(sbc);
                _rabbitMqEndpointConfigurator.Configure(sbc, host);
            });
        }

        private IRabbitMqHost Configure(IRabbitMqBusFactoryConfigurator sbc)
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