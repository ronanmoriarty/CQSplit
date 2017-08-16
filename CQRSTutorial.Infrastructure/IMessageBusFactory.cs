using System;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Infrastructure
{
    public interface IMessageBusFactory
    {
        IBusControl Create(Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> configure = null);
    }
}