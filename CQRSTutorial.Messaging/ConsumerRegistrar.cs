using System;
using MassTransit;

namespace CQRSTutorial.Messaging
{
    public class ConsumerRegistrar : IConsumerRegistrar
    {
        private readonly IConsumerFactory _consumerFactory;

        public ConsumerRegistrar(IConsumerFactory consumerFactory)
        {
            _consumerFactory = consumerFactory;
        }

        public void RegisterConsumerType(IReceiveEndpointConfigurator endpointConfigurator, Type consumerType)
        {
            endpointConfigurator.Consumer(consumerType, _consumerFactory.Create);
        }
    }
}