using System;

namespace CQSplit.Messaging
{
    public interface IConsumerRegistrar
    {
        void RegisterAllConsumerTypes(Action<ReceiveEndpointArgs> configure);
    }
}