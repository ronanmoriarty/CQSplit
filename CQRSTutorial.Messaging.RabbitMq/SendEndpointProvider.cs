using System;
using System.Threading.Tasks;
using MassTransit;

namespace CQRSTutorial.Messaging.RabbitMq
{
    public class SendEndpointProvider : ISendEndpointProvider
    {
        private readonly IBusControl _busControl;
        private readonly IRabbitMqHostConfiguration _rabbitMqHostConfiguration;

        public SendEndpointProvider(IBusControl busControl, IRabbitMqHostConfiguration rabbitMqHostConfiguration)
        {
            _busControl = busControl;
            _rabbitMqHostConfiguration = rabbitMqHostConfiguration;
        }

        public async Task<ISendEndpoint> GetSendEndpoint()
        {
            var uri = _rabbitMqHostConfiguration.Uri.AbsoluteUri;
            return await _busControl.GetSendEndpoint(new Uri(uri));
        }
    }
}