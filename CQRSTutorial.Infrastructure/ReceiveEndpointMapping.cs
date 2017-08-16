using System;

namespace CQRSTutorial.Infrastructure
{
    public class ReceiveEndpointMapping
    {
        public ReceiveEndpointMapping(
            string serviceAddress,
            Type consumerType)
        {
            ServiceAddress = serviceAddress;
            ConsumerType = consumerType;
        }

        public string ServiceAddress { get; }
        public Type ConsumerType { get; }
    }
}