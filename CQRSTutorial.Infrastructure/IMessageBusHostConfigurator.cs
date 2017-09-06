using System;

namespace CQRSTutorial.Infrastructure
{
    public interface IMessageBusHostConfigurator
    {
        Uri Uri { get; }
        string Username { get; }
        string Password { get; }
    }
}