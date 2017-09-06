using System;
using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Infrastructure
{
    public interface IRabbitMqHostConfigurator
    {
        Uri Uri { get; }
        IRabbitMqHost Configure(IRabbitMqBusFactoryConfigurator sbc);
    }
}