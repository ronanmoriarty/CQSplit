using System;
using System.Threading.Tasks;
using log4net;
using MassTransit;

namespace CQRSTutorial.Infrastructure
{
    public class EndpointProvider : IEndpointProvider
    {
        private readonly IBusControl _busControl;
        private readonly IMessageBusConfiguration _messageBusConfiguration;
        private readonly IServiceAddressProvider _serviceAddressProvider;
        private readonly ILog _logger = LogManager.GetLogger(typeof(EndpointProvider));

        public EndpointProvider(IBusControl busControl, IMessageBusConfiguration messageBusConfiguration, IServiceAddressProvider serviceAddressProvider)
        {
            _busControl = busControl;
            _messageBusConfiguration = messageBusConfiguration;
            _serviceAddressProvider = serviceAddressProvider;
        }

        public async Task<ISendEndpoint> GetSendEndpointFor<TCommand>()
        {
            var serviceAddress = _serviceAddressProvider.GetServiceAddressFor<TCommand>();
            var uri = $"{_messageBusConfiguration.Uri.AbsoluteUri}{serviceAddress}";
            _logger.Debug($"Sending {typeof(TCommand).Name} command to {uri}");
            return await _busControl.GetSendEndpoint(new Uri(uri));
        }
    }
}