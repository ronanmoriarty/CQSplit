using System.Collections.Generic;
using CQ.Core;

namespace CQ.Publish
{
    public class OutboxToMessageBusEventHandler : IEventHandler
    {
        private readonly IOutboxToMessageBusPublisher _outboxToMessageBusPublisher;

        public OutboxToMessageBusEventHandler(IOutboxToMessageBusPublisher outboxToMessageBusPublisher)
        {
            _outboxToMessageBusPublisher = outboxToMessageBusPublisher;
        }

        public void Handle(IEnumerable<IEvent> events)
        {
            _outboxToMessageBusPublisher.PublishEvents(events);
        }
    }
}