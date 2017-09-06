using MassTransit;

namespace CQRSTutorial.Infrastructure
{
    public class RabbitMqMessageBusFactory : IMessageBusFactory
    {
        private readonly IMessageBusHostConfigurator _messageBusHostConfigurator;
        private readonly IMessageBusConfigurator _messageBusConfigurator;

        public RabbitMqMessageBusFactory(IMessageBusHostConfigurator messageBusHostConfigurator,
            IMessageBusConfigurator messageBusConfigurator)
        {
            _messageBusHostConfigurator = messageBusHostConfigurator;
            _messageBusConfigurator = messageBusConfigurator;
        }

        public IBusControl Create()
        {
            return Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = _messageBusHostConfigurator.Configure(sbc);
                _messageBusConfigurator.ConfigureEndpoints(sbc, host);
            });
        }
    }
}