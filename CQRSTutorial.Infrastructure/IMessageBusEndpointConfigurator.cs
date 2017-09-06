using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Infrastructure
{
    public interface IMessageBusEndpointConfigurator
    {
        void Configure(IRabbitMqBusFactoryConfigurator sbc, IRabbitMqHost host);
    }
}