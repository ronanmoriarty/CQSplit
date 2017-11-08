using System;
using MassTransit;

namespace CQRSTutorial.Messaging
{
    public class InMemoryMessageBusFactory : IMessageBusFactory
    {
        private readonly IConsumerRegistrar _consumerRegistrar;
        private IInMemoryBusFactoryConfigurator _inMemoryBusFactoryConfigurator;

        public InMemoryMessageBusFactory(IConsumerRegistrar consumerRegistrar)
        {
            _consumerRegistrar = consumerRegistrar;
        }

        public IBusControl Create()
        {
            return Bus.Factory.CreateUsingInMemory(ConfigureReceiveEndpoints);
        }

        private void ConfigureReceiveEndpoints(IInMemoryBusFactoryConfigurator inMemoryBusFactoryConfigurator)
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