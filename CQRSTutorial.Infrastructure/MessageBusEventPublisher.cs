using System;
using System.Collections.Generic;
using CQRSTutorial.Core;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace CQRSTutorial.Infrastructure
{
    public class MessageBusEventPublisher : IEventPublisher
    {
        private readonly IMessageBusFactory _messageBusFactory;

        public MessageBusEventPublisher(IMessageBusFactory messageBusFactory)
        {
            _messageBusFactory = messageBusFactory;
        }

        public Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> Configure  { get; set; }

        public void Publish(IEnumerable<IEvent> events)
        {
            var bus = _messageBusFactory.Create(Configure);
            bus.Start();
            foreach (var @event in events)
            {
                bus.Publish(@event);
            }
            bus.Stop();
        }
    }
}
