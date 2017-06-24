using System;
using System.Collections.Generic;
using CQRSTutorial.Core;
using MassTransit;

namespace CQRSTutorial.Infrastructure
{
    public class MessageBusEventPublisher : IEventPublisher
    {
        private readonly IBusControl _bus;

        public MessageBusEventPublisher()
        {
            _bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = sbc.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
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
