using System;
using MassTransit;

namespace CQRSTutorial.Messaging
{
    public interface IConsumerRegistrar
    {
        void RegisterConsumerType(IReceiveEndpointConfigurator endpointConfigurator, Type consumerType);
    }
}