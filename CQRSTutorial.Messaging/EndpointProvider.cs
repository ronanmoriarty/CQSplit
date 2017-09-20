using System;
using System.Threading.Tasks;
using log4net;
using MassTransit;

namespace CQRSTutorial.Messaging
{
    public class EndpointProvider : IEndpointProvider
    {
        private readonly IBusControl _busControl;
        private readonly IRabbitMqHostConfiguration _rabbitMqHostConfiguration;
        private readonly IServiceAddressProvider _serviceAddressProvider;
        private readonly ILog _logger = LogManager.GetLogger(typeof(EndpointProvider));

        public EndpointProvider(IBusControl busControl, IRabbitMqHostConfiguration rabbitMqHostConfiguration, IServiceAddressProvider serviceAddressProvider)
        {
            _busControl = busControl;
            _rabbitMqHostConfiguration = rabbitMqHostConfiguration;
            _serviceAddressProvider = serviceAddressProvider;
        }

        public async Task<ISendEndpoint> GetSendEndpointFor(Type messageType)
        {
            var serviceAddress = _serviceAddressProvider.GetServiceAddressFor(messageType);
            var uri = $"{_rabbitMqHostConfiguration.Uri.AbsoluteUri}{serviceAddress}";
            _logger.Debug($"Sending {messageType.Name} message to {uri}");
            return await _busControl.GetSendEndpoint(new Uri(uri));
        }
    }
}