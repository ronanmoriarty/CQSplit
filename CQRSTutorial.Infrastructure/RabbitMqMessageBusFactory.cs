using MassTransit;

namespace CQRSTutorial.Infrastructure
{
    public class RabbitMqMessageBusFactory : IMessageBusFactory
    {
        private readonly IRabbitMQHostConfigurator _rabbitMqHostConfigurator;
        private readonly IRabbitMQEndpointConfigurator _rabbitMqEndpointConfigurator;

        public RabbitMqMessageBusFactory(
            IRabbitMQHostConfigurator rabbitMqHostConfigurator,
            IRabbitMQEndpointConfigurator rabbitMqEndpointConfigurator)
        {
            _rabbitMqHostConfigurator = rabbitMqHostConfigurator;
            _rabbitMqEndpointConfigurator = rabbitMqEndpointConfigurator;
        }

        public IBusControl Create()
        {
            return Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = _rabbitMqHostConfigurator.Configure(sbc);
                _rabbitMqEndpointConfigurator.Configure(sbc, host);
            });
        }
    }
}