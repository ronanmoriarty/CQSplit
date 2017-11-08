using System;
using MassTransit;

namespace CQRSTutorial.Messaging
{
    public class InMemoryReceiveEndpointsConfigurator : IInMemoryReceiveEndpointsConfigurator
    {
        private readonly IConsumerRegistrar _consumerRegistrar;
        private readonly string _queueName;
        private IInMemoryBusFactoryConfigurator _inMemoryBusFactoryConfigurator;

        public InMemoryReceiveEndpointsConfigurator(IConsumerRegistrar consumerRegistrar, string queueName)
        {
            _consumerRegistrar = consumerRegistrar;
            _queueName = queueName;
        }

        public void Configure(IInMemoryBusFactoryConfigurator inMemoryBusFactoryConfigurator)
        {
            _inMemoryBusFactoryConfigurator = inMemoryBusFactoryConfigurator;
            _consumerRegistrar.RegisterAllConsumerTypes(Configure);
        }

        private void Configure(Action<IReceiveEndpointConfigurator> configure)
        {
            _inMemoryBusFactoryConfigurator.ReceiveEndpoint(_queueName, configure);
        }
    }
}