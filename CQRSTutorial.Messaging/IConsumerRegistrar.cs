using System;
using MassTransit;

namespace CQRSTutorial.Messaging
{
    public interface IConsumerRegistrar
    {
        void RegisterAllConsumerTypes<TBusFactoryConfigurator>(TBusFactoryConfigurator sbc,
            Action<TBusFactoryConfigurator, Action<IReceiveEndpointConfigurator>> configure
        )
            where TBusFactoryConfigurator : IBusFactoryConfigurator;
    }
}