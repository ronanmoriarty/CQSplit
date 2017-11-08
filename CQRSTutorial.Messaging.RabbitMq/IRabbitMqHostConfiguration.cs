using System;

namespace CQRSTutorial.Messaging.RabbitMq
{
    public interface IRabbitMqHostConfiguration
    {
        Uri Uri { get; }
        string Username { get; }
        string Password { get; }
    }
}