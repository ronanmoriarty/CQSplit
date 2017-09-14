using System;

namespace CQRSTutorial.Messaging
{
    public interface IRabbitMqHostConfiguration
    {
        Uri Uri { get; }
        string Username { get; }
        string Password { get; }
    }
}