using System;
using System.Threading.Tasks;
using MassTransit;
using NLog;

namespace CQSplit.Messaging.RabbitMq
{
    public class RabbitMqSendEndpointProvider : ISendEndpointProvider
    {
        private readonly IBusControl _busControl;
        private readonly IRabbitMqHostConfiguration _rabbitMqHostConfiguration;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

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