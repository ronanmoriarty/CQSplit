using System;
using System.Threading.Tasks;
using log4net;
using MassTransit;

namespace CQRSTutorial.Messaging.RabbitMq
{
    public class RabbitMqSendEndpointProvider : ISendEndpointProvider
    {
        private readonly IBusControl _busControl;
        private readonly IRabbitMqHostConfiguration _rabbitMqHostConfiguration;
        private readonly ILog _logger = LogManager.GetLogger(typeof(RabbitMqSendEndpointProvider));

        public RabbitMqSendEndpointProvider(IBusControl busControl, IRabbitMqHostConfiguration rabbitMqHostConfiguration)
        {
            _busControl = busControl;
            _rabbitMqHostConfiguration = rabbitMqHostConfiguration;
        }

        public async Task<ISendEndpoint> GetSendEndpoint(string queueName)
        {
            var uri = new Uri($"{_rabbitMqHostConfiguration.Uri}/{queueName}");
            _logger.Debug($"Getting SendEndpoint for {uri}");
            return await _busControl.GetSendEndpoint(uri);
        }
    }
}