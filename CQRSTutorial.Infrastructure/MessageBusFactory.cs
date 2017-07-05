using System;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Infrastructure
{
    public class MessageBusFactory
    {
        private readonly IMessageBusConfiguration _messageBusConfiguration;
        private readonly Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> _configure;

        public MessageBusFactory(IMessageBusConfiguration messageBusConfiguration, Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> configure = null)
        {
            _messageBusConfiguration = messageBusConfiguration;
            _configure = configure;
        }

        public IBusControl Create()
        {
            return Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = sbc.Host(_messageBusConfiguration.Uri, h =>
                {
                    h.Username(_messageBusConfiguration.Username);
                    h.Password(_messageBusConfiguration.Password);
                });

                _configure?.Invoke(sbc, host);
            });
        }
    }
}