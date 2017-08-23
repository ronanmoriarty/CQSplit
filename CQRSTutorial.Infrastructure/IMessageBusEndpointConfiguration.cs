using System.Collections.Generic;

namespace CQRSTutorial.Infrastructure
{
    public interface IMessageBusEndpointConfiguration
    {
        IEnumerable<ReceiveEndpointMapping> ReceiveEndpoints { get; }
    }
}