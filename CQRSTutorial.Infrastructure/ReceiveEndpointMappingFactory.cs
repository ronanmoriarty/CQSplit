using System;

namespace CQRSTutorial.Infrastructure
{
    public class ReceiveEndpointMappingFactory
    {
        private readonly IServiceAddressProvider _serviceAddressProvider;

        public ReceiveEndpointMappingFactory(IServiceAddressProvider serviceAddressProvider)
        {
            _serviceAddressProvider = serviceAddressProvider;
        }

        public ReceiveEndpointMapping CreateMappingFor(Type consumerType)
        {
            return new ReceiveEndpointMapping(_serviceAddressProvider.GetServiceAddressFor(consumerType, "consumer", "command"), consumerType);
        }
    }
}