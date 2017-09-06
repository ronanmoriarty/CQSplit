using System;
using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Infrastructure
{
    public interface IRabbitMQHostConfigurator
    {
        Uri Uri { get; }
        IRabbitMqHost Configure(IRabbitMqBusFactoryConfigurator sbc);
    }
}