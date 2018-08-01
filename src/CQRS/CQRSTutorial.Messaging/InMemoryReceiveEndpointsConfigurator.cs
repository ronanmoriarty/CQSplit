using MassTransit;
using NLog;

namespace CQRSTutorial.Messaging
{
    public class InMemoryReceiveEndpointsConfigurator : IInMemoryReceiveEndpointsConfigurator
    {
        private readonly IConsumerRegistrar _consumerRegistrar;
        private IInMemoryBusFactoryConfigurator _inMemoryBusFactoryConfigurator;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public InMemoryReceiveEndpointsConfigurator(IConsumerRegistrar consumerRegistrar)
        {
            _consumerRegistrar = consumerRegistrar;
        }

        public void Configure(IInMemoryBusFactoryConfigurator inMemoryBusFactoryConfigurator)
        {
            _inMemoryBusFactoryConfigurator = inMemoryBusFactoryConfigurator;
            _consumerRegistrar.RegisterAllConsumerTypes(Configure);
        }

        private void Configure(ReceiveEndpointArgs receiveEndpointArgs)
        {
            _logger.Debug($"Configure in-memory endpoint for {receiveEndpointArgs.QueueName}");
            _inMemoryBusFactoryConfigurator.ReceiveEndpoint(receiveEndpointArgs.QueueName, receiveEndpointArgs.Configure);
        }
    }
}