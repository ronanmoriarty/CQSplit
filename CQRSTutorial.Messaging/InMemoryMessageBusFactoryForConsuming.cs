using System;
using MassTransit;

namespace CQRSTutorial.Messaging
{
    public class InMemoryMessageBusFactoryForConsuming : IMessageBusFactory
    {
        private readonly IConsumerRegistrar _consumerRegistrar;
        private IInMemoryBusFactoryConfigurator _inMemoryBusFactoryConfigurator;

        public InMemoryMessageBusFactoryForConsuming(IConsumerRegistrar consumerRegistrar)
        {
            _consumerRegistrar = consumerRegistrar;
        }

        public IBusControl Create()
        {
            return Bus.Factory.CreateUsingInMemory(ConfigureEndpoints);
        }

        private void ConfigureEndpoints(IInMemoryBusFactoryConfigurator inMemoryBusFactoryConfigurator)
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