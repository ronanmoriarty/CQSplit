using System;
using MassTransit;

namespace CQRSTutorial.Messaging
{
    public class InMemoryMessageBusFactoryForConsuming : IMessageBusFactory
    {
        private readonly IConsumerRegistrar _consumerRegistrar;

        public InMemoryMessageBusFactoryForConsuming(IConsumerRegistrar consumerRegistrar)
        {
            _consumerRegistrar = consumerRegistrar;
        }

        public IBusControl Create()
        {
            return Bus.Factory.CreateUsingInMemory(ConfigureEndpoints);
        }

        private void ConfigureEndpoints(IInMemoryBusFactoryConfigurator sbc)
        {
            _consumerRegistrar.RegisterAllConsumerTypes(sbc, Configure);
        }

        private void Configure(IInMemoryBusFactoryConfigurator inMemoryBusFactoryConfigurator, Action<IReceiveEndpointConfigurator> configure)
        {
            inMemoryBusFactoryConfigurator.ReceiveEndpoint(null, configure);
        }
    }
}