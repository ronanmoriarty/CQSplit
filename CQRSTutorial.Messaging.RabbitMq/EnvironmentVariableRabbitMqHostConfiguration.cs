using System;

namespace CQRSTutorial.Messaging.RabbitMq
{
    public class EnvironmentVariableRabbitMqHostConfiguration : IRabbitMqHostConfiguration
    {
        public string Uri => Environment.GetEnvironmentVariable("RABBITMQ_URI");
        public string Username => Environment.GetEnvironmentVariable("RABBITMQ_USERNAME");
        public string Password => Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");
    }
}