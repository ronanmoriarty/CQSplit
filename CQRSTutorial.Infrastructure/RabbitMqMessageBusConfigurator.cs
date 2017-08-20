using MassTransit;
using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Infrastructure
{
    public class RabbitMqMessageBusConfigurator : IMessageBusConfigurator
    {
        private readonly IMessageBusEndpointConfiguration _messageBusEndpointConfiguration;
        private readonly IConsumerFactory _consumerFactory;

        public RabbitMqMessageBusConfigurator(IMessageBusEndpointConfiguration messageBusEndpointConfiguration,
            IConsumerFactory consumerFactory)
        {
            _messageBusEndpointConfiguration = messageBusEndpointConfiguration;
            _consumerFactory = consumerFactory;
        }

        public void ConfigureEndpoints(IRabbitMqBusFactoryConfigurator sbc, IRabbitMqHost host)
        {
            foreach (var endpoint in _messageBusEndpointConfiguration.ReceiveEndpoints)
            {
                sbc.ReceiveEndpoint(host, endpoint.ServiceAddress,
                    endpointConfigurator => { endpointConfigurator.Consumer(endpoint.ConsumerType, _consumerFactory.Create); });
            }
        }
    }
}
