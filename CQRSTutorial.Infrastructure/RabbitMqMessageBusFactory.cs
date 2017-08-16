using System;
using log4net;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Infrastructure
{
    public class RabbitMqMessageBusFactory : IMessageBusFactory
    {
        private readonly IMessageBusConfiguration _messageBusConfiguration;
        private readonly IMessageBusEndpointConfiguration _messageBusEndpointConfiguration;
        private readonly ILog _logger = LogManager.GetLogger(typeof(RabbitMqMessageBusFactory));

        public RabbitMqMessageBusFactory(IMessageBusConfiguration messageBusConfiguration,
            IMessageBusEndpointConfiguration messageBusEndpointConfiguration)
        {
            _messageBusConfiguration = messageBusConfiguration;
            _messageBusEndpointConfiguration = messageBusEndpointConfiguration;
        }

        public IBusControl Create(Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> configure = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = ConfigureHost(sbc);
                configure?.Invoke(sbc, host);
            });
        }

        private IRabbitMqHost ConfigureHost(IRabbitMqBusFactoryConfigurator sbc)
        {
            var hostAddress = _messageBusConfiguration.Uri;
            _logger.Debug($"Host address is: \"{hostAddress.AbsoluteUri}\"");
            var host = sbc.Host(hostAddress, h =>
            {
                h.Username(_messageBusConfiguration.Username);
                h.Password(_messageBusConfiguration.Password);
            });
            return host;
        }
    }
}