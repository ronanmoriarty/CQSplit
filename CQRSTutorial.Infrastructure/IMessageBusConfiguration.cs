using System;

namespace CQRSTutorial.Infrastructure
{
    public interface IMessageBusConfiguration
    {
        Uri Uri { get; }
        string Username { get; }
        string Password { get; }
    }
}