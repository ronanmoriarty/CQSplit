using System.Collections.Generic;
using CQRSTutorial.Core;
using MassTransit;

namespace CQRSTutorial.Infrastructure
{
    public class MessageBusEventPublisher : IEventPublisher
    {
        private readonly IBusControl _bus;

        public MessageBusEventPublisher(MessageBusFactory messageBusFactory)
        {
            _bus = messageBusFactory.Create();
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
