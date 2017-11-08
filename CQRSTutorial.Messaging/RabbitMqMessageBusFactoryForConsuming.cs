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
        private IRabbitMqBusFactoryConfigurator _rabbitMqBusFactoryConfigurator;

        public RabbitMqMessageBusFactoryForConsuming(
            IRabbitMqHostConfiguration rabbitMqHostConfiguration,
            IConsumerRegistrar consumerRegistrar)
        {
            _rabbitMqHostConfiguration = rabbitMqHostConfiguration;
            _consumerRegistrar = consumerRegistrar;
        }

        public IBusControl Create()
        {
            return Bus.Factory.CreateUsingRabbitMq(rabbitMqBusFactoryConfigurator =>
            {
                _host = CreateHost(rabbitMqBusFactoryConfigurator);
                ConfigureEndpoints(rabbitMqBusFactoryConfigurator);
            });
        }

        private IRabbitMqHost CreateHost(IRabbitMqBusFactoryConfigurator rabbitMqBusFactoryConfigurator)
        {
            var hostAddress = _rabbitMqHostConfiguration.Uri;
            _logger.Debug($"Host address is: \"{hostAddress.AbsoluteUri}\"");
            var host = rabbitMqBusFactoryConfigurator.Host(hostAddress, h =>
            {
                h.Username(_rabbitMqHostConfiguration.Username);
                h.Password(_rabbitMqHostConfiguration.Password);
            });

            return host;
        }

        private void ConfigureEndpoints(IRabbitMqBusFactoryConfigurator sbc)
        {
            _rabbitMqBusFactoryConfigurator = sbc;
            _consumerRegistrar.RegisterAllConsumerTypes(Configure);
        }

        private void Configure(Action<IReceiveEndpointConfigurator> configure)
        {
            _rabbitMqBusFactoryConfigurator.ReceiveEndpoint(_host, null, configure);
        }
    }
}