using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Messaging
{
    public interface IRabbitMqReceiveEndpointConfigurator
    {
        void Configure(IRabbitMqHost host, IRabbitMqBusFactoryConfigurator rabbitMqBusFactoryConfigurator);
    }
}