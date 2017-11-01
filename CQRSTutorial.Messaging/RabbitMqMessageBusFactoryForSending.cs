using log4net;
using MassTransit;

namespace CQRSTutorial.Messaging
{
    public class RabbitMqMessageBusFactoryForSending : IMessageBusFactory
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(RabbitMqMessageBusFactoryForSending));
        private readonly IRabbitMqHostConfiguration _rabbitMqHostConfiguration;

        public RabbitMqMessageBusFactoryForSending(
            IRabbitMqHostConfiguration rabbitMqHostConfiguration)
        {
            _rabbitMqHostConfiguration = rabbitMqHostConfiguration;
        }

        public IBusControl Create()
        {
            return Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var hostAddress = _rabbitMqHostConfiguration.Uri;
                _logger.Debug($"Host address is: \"{hostAddress.AbsoluteUri}\"");
                sbc.Host(hostAddress, h =>
                {
                    h.Username(_rabbitMqHostConfiguration.Username);
                    h.Password(_rabbitMqHostConfiguration.Password);
                });
            });
        }
    }
}