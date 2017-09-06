using System;

namespace CQRSTutorial.Infrastructure
{
    public class EnvironmentVariableMessageBusHostConfigurator : IMessageBusHostConfigurator
    {
        public Uri Uri => new Uri(Environment.GetEnvironmentVariable("RABBITMQ_URI"));
        public string Username => Environment.GetEnvironmentVariable("RABBITMQ_USERNAME");
        public string Password => Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");
    }
}