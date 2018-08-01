using MassTransit.RabbitMqTransport;
using NLog;

namespace CQRSTutorial.Messaging.RabbitMq
{
    public class RabbitMqReceiveEndpointConfigurator : IRabbitMqReceiveEndpointConfigurator
    {
        private readonly IConsumerRegistrar _consumerRegistrar;
        private IRabbitMqHost _host;
        private IRabbitMqBusFactoryConfigurator _rabbitMqBusFactoryConfigurator;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public RabbitMqReceiveEndpointConfigurator(IConsumerRegistrar consumerRegistrar)
        {
            _consumerRegistrar = consumerRegistrar;
        }

        public void Configure(IRabbitMqHost host, IRabbitMqBusFactoryConfigurator rabbitMqBusFactoryConfigurator)
        {
            _host = host;
            _rabbitMqBusFactoryConfigurator = rabbitMqBusFactoryConfigurator;
            _consumerRegistrar.RegisterAllConsumerTypes(Configure);
        }

        private void Configure(ReceiveEndpointArgs receiveEndpointArgs)
        {
            _logger.Debug($"Configure RabbitMq endpoint for {receiveEndpointArgs.QueueName} on {_host.Settings.Host}");
            _rabbitMqBusFactoryConfigurator.ReceiveEndpoint(_host, receiveEndpointArgs.QueueName, receiveEndpointArgs.Configure);
        }
    }
}