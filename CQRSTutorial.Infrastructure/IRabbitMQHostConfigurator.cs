using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Infrastructure
{
    public interface IRabbitMqHostConfigurator
    {
        IRabbitMqHost Configure(IRabbitMqBusFactoryConfigurator sbc);
    }
}