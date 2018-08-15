using System;
using MassTransit;
using MassTransit.NLogIntegration;
using MassTransit.RabbitMqTransport;
using NLog;

namespace CQSplit.Messaging.RabbitMq
{
    public class RabbitMqMessageBusFactory : IMessageBusFactory
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
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
            rabbitMqBusFactoryConfigurator.UseNLog();
            _logger.Debug($"Host address is: \"{hostAddress.AbsoluteUri}\"");
            _logger.Debug($"Username is: \"{_rabbitMqHostConfiguration.Username}\"");
            return rabbitMqBusFactoryConfigurator.Host(hostAddress, h =>
            {
                h.Username(_rabbitMqHostConfiguration.Username);
                h.Password(_rabbitMqHostConfiguration.Password);
            });
        }
    }
}