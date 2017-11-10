using System;

namespace CQRSTutorial.Messaging
{
    public interface IConsumerRegistrar
    {
        void RegisterAllConsumerTypes(Action<ReceiveEndpointArgs> configure);
    }
}