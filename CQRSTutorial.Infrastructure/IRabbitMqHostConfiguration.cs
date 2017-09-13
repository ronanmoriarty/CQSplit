using System;

namespace CQRSTutorial.Infrastructure
{
    public interface IRabbitMqHostConfiguration
    {
        Uri Uri { get; }
        string Username { get; }
        string Password { get; }
    }
}