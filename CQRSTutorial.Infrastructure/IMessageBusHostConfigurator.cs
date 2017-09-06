using System;
using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Infrastructure
{
    public interface IMessageBusHostConfigurator
    {
        Uri Uri { get; }
        IRabbitMqHost Configure(IRabbitMqBusFactoryConfigurator sbc);
    }
}