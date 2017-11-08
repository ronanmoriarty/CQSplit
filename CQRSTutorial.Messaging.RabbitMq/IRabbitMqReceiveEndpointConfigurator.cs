using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Messaging.RabbitMq
{
    public interface IRabbitMqReceiveEndpointConfigurator
    {
        void Configure(IRabbitMqHost host, IRabbitMqBusFactoryConfigurator rabbitMqBusFactoryConfigurator);
    }
}