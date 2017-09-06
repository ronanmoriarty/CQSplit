using MassTransit;

namespace CQRSTutorial.Infrastructure
{
    public class RabbitMqMessageBusFactory : IMessageBusFactory
    {
        private readonly IMessageBusHostConfigurator _messageBusHostConfigurator;
        private readonly IMessageBusEndpointConfigurator _messageBusEndpointConfigurator;

        public RabbitMqMessageBusFactory(IMessageBusHostConfigurator messageBusHostConfigurator,
            IMessageBusEndpointConfigurator messageBusEndpointConfigurator)
        {
            _messageBusHostConfigurator = messageBusHostConfigurator;
            _messageBusEndpointConfigurator = messageBusEndpointConfigurator;
        }

        public IBusControl Create()
        {
            return Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = _messageBusHostConfigurator.Configure(sbc);
                _messageBusEndpointConfigurator.Configure(sbc, host);
            });
        }
    }
}