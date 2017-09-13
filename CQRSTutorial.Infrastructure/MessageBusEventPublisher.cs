using System.Collections.Generic;
using CQRSTutorial.Core;
using MassTransit;

namespace CQRSTutorial.Infrastructure
{
    public class MessageBusEventPublisher : IEventHandler
    {
        private readonly IMessageBusFactory _messageBusFactory;

        public MessageBusEventPublisher(IMessageBusFactory messageBusFactory)
        {
            _messageBusFactory = messageBusFactory;
        }

        public void Handle(IEnumerable<IEvent> events)
        {
            var bus = _messageBusFactory.Create();
            bus.Start();
            foreach (var @event in events)
            {
                bus.Publish(@event);
            }
            bus.Stop();
        }
    }
}
