using CQRSTutorial.Messaging.RabbitMq;
using Microsoft.Extensions.Configuration;

namespace Cafe.Waiter.Web
{
    public class RabbitMqHostConfiguration : IRabbitMqHostConfiguration
    {
        private readonly IConfiguration _configuration;

        public RabbitMqHostConfiguration(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public string Uri => _configuration["RabbitMq:Uri"];
        public string Username => _configuration["RabbitMq:Username"];
        public string Password => _configuration["RabbitMq:Password"];
    }
}
