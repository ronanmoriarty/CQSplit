using System;
using MassTransit;

namespace CQRSTutorial.Messaging
{
    public interface IConsumerRegistrar
    {
        void RegisterConsumerType(IReceiveEndpointConfigurator endpointConfigurator, Type consumerType);
        void RegisterAllConsumerTypes<TBusFactoryConfigurator>(TBusFactoryConfigurator sbc, Action<TBusFactoryConfigurator, Type> action) where TBusFactoryConfigurator : IBusFactoryConfigurator;
    }
}