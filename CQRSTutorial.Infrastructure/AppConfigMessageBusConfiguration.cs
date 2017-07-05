using System;
using System.Configuration;

namespace CQRSTutorial.Infrastructure
{
    public class AppConfigMessageBusConfiguration : IMessageBusConfiguration
    {
        public Uri Uri => new Uri(ConfigurationManager.AppSettings["RabbitMQUri"]);
        public string Username => ConfigurationManager.AppSettings["RabbitMQUsername"];
        public string Password => ConfigurationManager.AppSettings["RabbitMQPassword"];
    }
}