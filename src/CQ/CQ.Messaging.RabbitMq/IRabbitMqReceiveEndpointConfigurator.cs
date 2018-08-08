using MassTransit.RabbitMqTransport;

namespace CQ.Messaging.RabbitMq
{
    public interface IRabbitMqReceiveEndpointConfigurator
    {
        void Configure(IRabbitMqHost host, IRabbitMqBusFactoryConfigurator rabbitMqBusFactoryConfigurator);
    }
}