using System;
using System.Threading.Tasks;
using MassTransit;

namespace CQRSTutorial.Messaging.RabbitMq
{
    public class RabbitMqSendEndpointProvider : ISendEndpointProvider
    {
        private readonly IBusControl _busControl;
        private readonly IRabbitMqHostConfiguration _rabbitMqHostConfiguration;

        public RabbitMqSendEndpointProvider(IBusControl busControl, IRabbitMqHostConfiguration rabbitMqHostConfiguration)
        {
            _busControl = busControl;
            _rabbitMqHostConfiguration = rabbitMqHostConfiguration;
        }

        public async Task<ISendEndpoint> GetSendEndpoint(string queueName)
        {
            var uri = new Uri($"{_rabbitMqHostConfiguration.Uri}/{queueName}");
            return await _busControl.GetSendEndpoint(uri);
        }
    }
}