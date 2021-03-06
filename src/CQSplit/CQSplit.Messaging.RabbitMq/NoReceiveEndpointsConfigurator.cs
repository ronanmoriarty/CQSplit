using MassTransit.RabbitMqTransport;

namespace CQSplit.Messaging.RabbitMq
{
    public class NoReceiveEndpointsConfigurator : IRabbitMqReceiveEndpointConfigurator
    {
        public static NoReceiveEndpointsConfigurator Instance { get; } = new NoReceiveEndpointsConfigurator();

        private NoReceiveEndpointsConfigurator()
        {
        }

        public void Configure(IRabbitMqHost host, IRabbitMqBusFactoryConfigurator rabbitMqBusFactoryConfigurator)
        {
        }
    }
}