using System;
using System.Collections.Generic;

namespace CQRSTutorial.Messaging
{
    public interface IMessageBusEndpointConfiguration
    {
        List<Type> GetConsumerTypes();
    }
}