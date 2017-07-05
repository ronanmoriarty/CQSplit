using System;
using System.Collections.Generic;
using CQRSTutorial.Core;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Infrastructure
{
    public class MessageBusEventPublisher : IEventPublisher
    {
        private readonly IBusControl _bus;

        public MessageBusEventPublisher(IMessageBusConfiguration messageBusConfiguration, Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> configure = null)
        {
            _bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = sbc.Host(messageBusConfiguration.Uri, h =>
                {
                    h.Username(messageBusConfiguration.Username);
                    h.Password(messageBusConfiguration.Password);
                });

                configure?.Invoke(sbc, host);
            });

            _bus.Start();
        }

        public void Publish(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                _bus.Publish(@event);
            }
        }

        public void Stop()
        {
            _bus.Stop();
        }
    }
}
