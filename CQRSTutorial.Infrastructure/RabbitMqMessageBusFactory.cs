using log4net;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Infrastructure
{
    public class RabbitMqMessageBusFactory : IMessageBusFactory
    {
        private readonly IMessageBusHostConfigurator _messageBusHostConfigurator;
        private readonly IMessageBusConfigurator _messageBusConfigurator;
        private readonly ILog _logger = LogManager.GetLogger(typeof(RabbitMqMessageBusFactory));

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
                var host = ConfigureHost(sbc);
                _messageBusConfigurator.ConfigureEndpoints(sbc, host);
            });
        }

        private IRabbitMqHost ConfigureHost(IRabbitMqBusFactoryConfigurator sbc)
        {
            var hostAddress = _messageBusHostConfigurator.Uri;
            _logger.Debug($"Host address is: \"{hostAddress.AbsoluteUri}\"");
            var host = sbc.Host(hostAddress, h =>
            {
                h.Username(_messageBusHostConfigurator.Username);
                h.Password(_messageBusHostConfigurator.Password);
            });
            return host;
        }
    }
}