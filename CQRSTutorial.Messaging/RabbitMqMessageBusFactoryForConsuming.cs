using log4net;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Messaging
{
    public class RabbitMqMessageBusFactoryForConsuming : IMessageBusFactory
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(RabbitMqMessageBusFactoryForConsuming));
        private readonly IConsumerTypeProvider _consumerTypeProvider;
        private readonly IConsumerRegistrar _consumerRegistrar;
        private readonly IRabbitMqHostConfiguration _rabbitMqHostConfiguration;

        public RabbitMqMessageBusFactoryForConsuming(
            IRabbitMqHostConfiguration rabbitMqHostConfiguration,
            IConsumerTypeProvider consumerTypeProvider,
            IConsumerRegistrar consumerRegistrar)
        {
            _rabbitMqHostConfiguration = rabbitMqHostConfiguration;
            _consumerTypeProvider = consumerTypeProvider;
            _consumerRegistrar = consumerRegistrar;
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
            foreach (var consumerType in _consumerTypeProvider.GetConsumerTypes())
            {
                sbc.ReceiveEndpoint(host, null,
                    endpointConfigurator => { _consumerRegistrar.RegisterConsumerType(endpointConfigurator, consumerType); });
            }
        }
    }
}