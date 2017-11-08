using System;
using log4net;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Messaging
{
    public class RabbitMqMessageBusFactoryForConsuming : IMessageBusFactory
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(RabbitMqMessageBusFactoryForConsuming));
        private readonly IConsumerRegistrar _consumerRegistrar;
        private readonly IRabbitMqHostConfiguration _rabbitMqHostConfiguration;
        private IRabbitMqHost _host;

        public RabbitMqMessageBusFactoryForConsuming(
            IRabbitMqHostConfiguration rabbitMqHostConfiguration,
            IConsumerRegistrar consumerRegistrar)
        {
            _rabbitMqHostConfiguration = rabbitMqHostConfiguration;
            _consumerRegistrar = consumerRegistrar;
        }

        public IBusControl Create()
        {
            return Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                _host = CreateHost(sbc);
                ConfigureEndpoints(sbc);
            });
        }

        private IRabbitMqHost CreateHost(IRabbitMqBusFactoryConfigurator sbc)
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

        private void ConfigureEndpoints(IRabbitMqBusFactoryConfigurator sbc)
        {
            _consumerRegistrar.RegisterAllConsumerTypes(sbc, Configure);
        }

        private void Configure(IRabbitMqBusFactoryConfigurator rabbitMqBusFactoryConfigurator, Action<IReceiveEndpointConfigurator> configure)
        {
            rabbitMqBusFactoryConfigurator.ReceiveEndpoint(_host, null, configure);
        }
    }
}