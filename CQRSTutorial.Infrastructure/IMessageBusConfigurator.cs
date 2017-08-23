using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Infrastructure
{
    public interface IMessageBusConfigurator
    {
        void ConfigureEndpoints(IRabbitMqBusFactoryConfigurator sbc, IRabbitMqHost host);
    }
}