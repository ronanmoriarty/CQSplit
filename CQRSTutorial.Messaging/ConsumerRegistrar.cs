using System;
using MassTransit;

namespace CQRSTutorial.Messaging
{
    public class ConsumerRegistrar : IConsumerRegistrar
    {
        private readonly IConsumerFactory _consumerFactory;
        private readonly IConsumerTypeProvider _consumerTypeProvider;

        public ConsumerRegistrar(IConsumerFactory consumerFactory, IConsumerTypeProvider consumerTypeProvider)
        {
            _consumerFactory = consumerFactory;
            _consumerTypeProvider = consumerTypeProvider;
        }

        private void RegisterConsumerType(IReceiveEndpointConfigurator endpointConfigurator, Type consumerType)
        {
            endpointConfigurator.Consumer(consumerType, _consumerFactory.Create);
        }

        public void RegisterAllConsumerTypes(Action<Action<IReceiveEndpointConfigurator>> configure)
        {
            foreach (var consumerType in _consumerTypeProvider.GetConsumerTypes())
            {
                configure(receiveEndpointConfigurator => RegisterConsumerType(receiveEndpointConfigurator, consumerType));
            }
        }
    }
}