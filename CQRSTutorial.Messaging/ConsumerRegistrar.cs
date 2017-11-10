using System;
using MassTransit;

namespace CQRSTutorial.Messaging
{
    public class ConsumerRegistrar : IConsumerRegistrar
    {
        private readonly IConsumerFactory _consumerFactory;
        private readonly IConsumerTypeProvider _consumerTypeProvider;
        private readonly IReceiveEndpointConfiguration _receiveEndpointConfiguration;

        public ConsumerRegistrar(IConsumerFactory consumerFactory,
            IConsumerTypeProvider consumerTypeProvider,
            IReceiveEndpointConfiguration receiveEndpointConfiguration)
        {
            _consumerFactory = consumerFactory;
            _consumerTypeProvider = consumerTypeProvider;
            _receiveEndpointConfiguration = receiveEndpointConfiguration;
        }

        public void RegisterAllConsumerTypes(Action<Action<IReceiveEndpointConfigurator>> configure)
        {
            foreach (var consumerType in _consumerTypeProvider.GetConsumerTypes())
            {
                configure(receiveEndpointConfigurator => RegisterConsumerType(receiveEndpointConfigurator, consumerType));
            }
        }

        public void RegisterAllConsumerTypes(Action<ReceiveEndpointArgs> configure)
        {
            foreach (var consumerType in _consumerTypeProvider.GetConsumerTypes())
            {
                configure(new ReceiveEndpointArgs(_receiveEndpointConfiguration.QueueName, receiveEndpointConfigurator => RegisterConsumerType(receiveEndpointConfigurator, consumerType)));
            }
        }

        private void RegisterConsumerType(IReceiveEndpointConfigurator receiveEndpointConfigurator, Type consumerType)
        {
            receiveEndpointConfigurator.Consumer(consumerType, _consumerFactory.Create);
        }
    }
}