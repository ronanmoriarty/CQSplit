using System;
using MassTransit;

namespace CQRSTutorial.Messaging
{
    public interface IConsumerRegistrar
    {
        void RegisterAllConsumerTypes(Action<Action<IReceiveEndpointConfigurator>> configure);
    }
}