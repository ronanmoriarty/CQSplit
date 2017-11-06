using log4net;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Messaging
{
    public class RabbitMqMessageBusFactoryForConsuming : IMessageBusFactory
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(RabbitMqMessageBusFactoryForConsuming));
        private readonly IMessageBusEndpointConfiguration _messageBusEndpointConfiguration;
        private readonly IConsumerFactory _consumerFactory;
        private readonly IRabbitMqHostConfiguration _rabbitMqHostConfiguration;

        public RabbitMqMessageBusFactoryForConsuming(
            IRabbitMqHostConfiguration rabbitMqHostConfiguration,
            IMessageBusEndpointConfiguration messageBusEndpointConfiguration,
            IConsumerFactory consumerFactory)
        {
            _rabbitMqHostConfiguration = rabbitMqHostConfiguration;
            _messageBusEndpointConfiguration = messageBusEndpointConfiguration;
            _consumerFactory = consumerFactory;
        }

        public IBusControl Create()
        {
            return Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = CreateHost(sbc);
                ConfigureEndpoints(sbc, host);
            });
        }

        private IRabbitMqHost CreateHost(IRabbitMqBusFactoryConfigurator sbc)
        {
            var hostAddress = _rabbitMqHostConfiguration.Uri;
            _logger.Debug($"Host address is: \"{hostAddress.AbsoluteUri}\"");
            var host = sbc.Host(hostAddress, h =>
            {
                h.Username(_rabbitMqHostConfiguration.Username);
                h.Password(_rabbitMqHostConfiguration.Password);
            });

            return host;
        }

        private void ConfigureEndpoints(IRabbitMqBusFactoryConfigurator sbc, IRabbitMqHost host)
        {
            foreach (var consumerType in _messageBusEndpointConfiguration.GetConsumerTypes())
            {
                sbc.ReceiveEndpoint(host, null,
                    endpointConfigurator => { endpointConfigurator.Consumer(consumerType, _consumerFactory.Create); });
            }
        }
    }
}