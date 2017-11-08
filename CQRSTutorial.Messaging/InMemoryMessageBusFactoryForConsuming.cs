using MassTransit;

namespace CQRSTutorial.Messaging
{
    public class InMemoryMessageBusFactoryForConsuming : IMessageBusFactory
    {
        private readonly IConsumerTypeProvider _consumerTypeProvider;
        private readonly IConsumerFactory _consumerFactory;

        public InMemoryMessageBusFactoryForConsuming(
            IConsumerTypeProvider consumerTypeProvider,
            IConsumerFactory consumerFactory)
        {
            _consumerTypeProvider = consumerTypeProvider;
            _consumerFactory = consumerFactory;
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
                    endpointConfigurator => { endpointConfigurator.Consumer(consumerType, _consumerFactory.Create); });
            }
        }
    }
}