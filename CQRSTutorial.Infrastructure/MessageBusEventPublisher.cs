using System;
using System.Collections.Generic;
using CQRSTutorial.Core;
using MassTransit;

namespace CQRSTutorial.Infrastructure
{
    public class MessageBusEventPublisher : IEventPublisher
    {
        private readonly IBusControl _bus;

        public MessageBusEventPublisher(IMessageBusConfiguration messageBusConfiguration)
        {
            _bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = sbc.Host(messageBusConfiguration.Uri, h =>
                {
                    h.Username(messageBusConfiguration.Username);
                    h.Password(messageBusConfiguration.Password);
                });

                sbc.ReceiveEndpoint(host, "test_queue", ep =>
                {
                    ep.Handler<IEvent> (context =>
                    {
                        var messages = string.Join("\n", context.Message);
                        return Console.Out.WriteLineAsync($"Received: {messages}");
                    });
                });
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
