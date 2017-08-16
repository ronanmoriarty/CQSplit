using System;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Infrastructure
{
    public class RabbitMqMessageBusConfigurator : IMessageBusConfigurator
    {
        private readonly IMessageBusEndpointConfiguration _messageBusEndpointConfiguration;

        public RabbitMqMessageBusConfigurator(IMessageBusEndpointConfiguration messageBusEndpointConfiguration)
        {
            _messageBusEndpointConfiguration = messageBusEndpointConfiguration;
        }

        public void ConfigureEndpoints(IRabbitMqBusFactoryConfigurator sbc, IRabbitMqHost host)
        {
            foreach (var endpoint in _messageBusEndpointConfiguration.ReceiveEndpoints)
            {
                sbc.ReceiveEndpoint(host, endpoint.ServiceAddress,
                    endpointConfigurator => { endpointConfigurator.Consumer(endpoint.ConsumerType, Activator.CreateInstance); });
            }
        }
    }
}
