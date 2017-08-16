using System;
using log4net;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Infrastructure
{
    public class MessageBusFactory : IMessageBusFactory
    {
        private readonly IMessageBusConfiguration _messageBusConfiguration;
        private readonly ILog _logger = LogManager.GetLogger(typeof(MessageBusFactory));

        public MessageBusFactory(IMessageBusConfiguration messageBusConfiguration)
        {
            _messageBusConfiguration = messageBusConfiguration;
        }

        public IBusControl Create(Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> configure = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var hostAddress = _messageBusConfiguration.Uri;
                _logger.Debug($"Host address is: \"{hostAddress.AbsoluteUri }\"");
                var host = sbc.Host(hostAddress, h =>
                {
                    h.Username(_messageBusConfiguration.Username);
                    h.Password(_messageBusConfiguration.Password);
                });

                configure?.Invoke(sbc, host);
            });
        }
    }
}