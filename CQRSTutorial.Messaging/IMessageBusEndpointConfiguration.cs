using System;
using System.Collections.Generic;

namespace CQRSTutorial.Messaging
{
    public interface IMessageBusEndpointConfiguration
    {
        IEnumerable<ReceiveEndpointMapping> ReceiveEndpoints { get; }
        List<Type> GetConsumerTypes();
    }
}