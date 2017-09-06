using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Infrastructure
{
    public interface IRabbitMQEndpointConfigurator
    {
        void Configure(IRabbitMqBusFactoryConfigurator sbc, IRabbitMqHost host);
    }
}