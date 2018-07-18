using System;
using log4net;
using MassTransit;
using MassTransit.Log4NetIntegration;
using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Messaging.RabbitMq
{
    public class RabbitMqMessageBusFactory : IMessageBusFactory
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(RabbitMqMessageBusFactory));
        private readonly IRabbitMqReceiveEndpointConfigurator _rabbitMqReceiveEndpointConfigurator;
        private readonly IRabbitMqHostConfiguration _rabbitMqHostConfiguration;
        private IRabbitMqHost _host;

        public RabbitMqMessageBusFactory(
            IRabbitMqHostConfiguration rabbitMqHostConfiguration,
            IRabbitMqReceiveEndpointConfigurator rabbitMqReceiveEndpointConfigurator)
        {
            _rabbitMqHostConfiguration = rabbitMqHostConfiguration;
            _rabbitMqReceiveEndpointConfigurator = rabbitMqReceiveEndpointConfigurator;
        }

        public IBusControl Create()
        {
            return Bus.Factory.CreateUsingRabbitMq(rabbitMqBusFactoryConfigurator =>
            {
                _host = CreateHost(rabbitMqBusFactoryConfigurator);
                _rabbitMqReceiveEndpointConfigurator.Configure(_host, rabbitMqBusFactoryConfigurator);
            });
        }

        private IRabbitMqHost CreateHost(IRabbitMqBusFactoryConfigurator rabbitMqBusFactoryConfigurator)
        {
            var hostAddress = new Uri(_rabbitMqHostConfiguration.Uri);
            _logger.Debug($"Host address is: \"{hostAddress.AbsoluteUri}\"");
            rabbitMqBusFactoryConfigurator.UseLog4Net();
            return rabbitMqBusFactoryConfigurator.Host(hostAddress, h =>
            {
                h.Username(_rabbitMqHostConfiguration.Username);
                h.Password(_rabbitMqHostConfiguration.Password);
            });
        }
    }
}