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
        private readonly ILog _logger = LogManager.GetLogger(typeof(EndpointProvider));

        public EndpointProvider(IBusControl busControl, IRabbitMqHostConfiguration rabbitMqHostConfiguration)
        {
            _busControl = busControl;
            _rabbitMqHostConfiguration = rabbitMqHostConfiguration;
        }

        public async Task<ISendEndpoint> GetSendEndpointFor(Type messageType)
        {
            var uri = $"{_rabbitMqHostConfiguration.Uri.AbsoluteUri}";
            _logger.Debug($"Sending {messageType.Name} message to {uri}");
            return await _busControl.GetSendEndpoint(new Uri(uri));
        }
    }
}