using MassTransit;

namespace CQRSTutorial.Messaging
{
    public class InMemoryMessageBusFactoryForConsuming : IMessageBusFactory
    {
        private readonly IConsumerTypeProvider _consumerTypeProvider;
        private readonly IConsumerRegistrar _consumerRegistrar;

        public InMemoryMessageBusFactoryForConsuming(
            IConsumerTypeProvider consumerTypeProvider,
            IConsumerRegistrar consumerRegistrar)
        {
            _consumerTypeProvider = consumerTypeProvider;
            _consumerRegistrar = consumerRegistrar;
        }

        public IBusControl Create()
        {
            return Bus.Factory.CreateUsingInMemory(ConfigureEndpoints);
        }

        private void ConfigureEndpoints(IInMemoryBusFactoryConfigurator sbc)
        {
            foreach (var consumerType in _consumerTypeProvider.GetConsumerTypes())
            {
                sbc.ReceiveEndpoint(null,
                    endpointConfigurator => { _consumerRegistrar.RegisterConsumerType(endpointConfigurator, consumerType); });
            }
        }
    }
}