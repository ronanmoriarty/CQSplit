using System;

namespace CQ.Messaging
{
    public interface IConsumerRegistrar
    {
        void RegisterAllConsumerTypes(Action<ReceiveEndpointArgs> configure);
    }
}