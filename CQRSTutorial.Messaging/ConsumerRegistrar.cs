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

        public void RegisterConsumerType(IReceiveEndpointConfigurator endpointConfigurator, Type consumerType)
        {
            endpointConfigurator.Consumer(consumerType, _consumerFactory.Create);
        }

        public void RegisterAllConsumerTypes<TBusFactoryConfigurator>(TBusFactoryConfigurator sbc, Action<TBusFactoryConfigurator, Type> action)
            where TBusFactoryConfigurator : IBusFactoryConfigurator
        {
            foreach (var consumerType in _consumerTypeProvider.GetConsumerTypes())
            {
                action(sbc, consumerType);
            }
        }

        public void RegisterAllConsumerTypes<TBusFactoryConfigurator>(TBusFactoryConfigurator sbc, Action<TBusFactoryConfigurator, Action<IReceiveEndpointConfigurator>> action) where TBusFactoryConfigurator : IBusFactoryConfigurator
        {
            foreach (var consumerType in _consumerTypeProvider.GetConsumerTypes())
            {
                action(sbc, receiveEndpointConfigurator => RegisterConsumerType(receiveEndpointConfigurator, consumerType));}
        }
    }
}