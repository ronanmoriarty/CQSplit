using CQRSTutorial.Messaging.RabbitMq;
using Microsoft.Extensions.Configuration;

namespace Cafe.Waiter.Web
{
    public class RabbitMqHostConfiguration : IRabbitMqHostConfiguration
    {
        private readonly IConfigurationSection _configurationSection;

        public RabbitMqHostConfiguration(IConfigurationRoot configuration)
        {
            _configurationSection = configuration.GetSection("RabbitMq");
        }

        public string Uri => _configurationSection["Uri"];
        public string Username => _configurationSection["Username"];
        public string Password => _configurationSection["Password"];
    }
}
