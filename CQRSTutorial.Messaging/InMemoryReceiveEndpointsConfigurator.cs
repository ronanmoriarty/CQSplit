using System;
using MassTransit;

namespace CQRSTutorial.Messaging
{
    public class InMemoryReceiveEndpointsConfigurator : IInMemoryReceiveEndpointsConfigurator
    {
        private readonly IConsumerRegistrar _consumerRegistrar;
        private IInMemoryBusFactoryConfigurator _inMemoryBusFactoryConfigurator;

        public InMemoryReceiveEndpointsConfigurator(IConsumerRegistrar consumerRegistrar)
        {
            _consumerRegistrar = consumerRegistrar;
        }

        public void Configure(IInMemoryBusFactoryConfigurator inMemoryBusFactoryConfigurator)
        {
            _inMemoryBusFactoryConfigurator = inMemoryBusFactoryConfigurator;
            _consumerRegistrar.RegisterAllConsumerTypes(Configure);
        }

        private void Configure(Action<IReceiveEndpointConfigurator> configure)
        {
            _inMemoryBusFactoryConfigurator.ReceiveEndpoint(null, configure);
        }
    }
}