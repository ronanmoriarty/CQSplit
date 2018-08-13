using MassTransit.RabbitMqTransport;

namespace CQSplit.Messaging.RabbitMq
{
    public interface IRabbitMqReceiveEndpointConfigurator
    {
        void Configure(IRabbitMqHost host, IRabbitMqBusFactoryConfigurator rabbitMqBusFactoryConfigurator);
    }
}