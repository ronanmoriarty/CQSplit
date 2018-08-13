using System.Collections.Generic;
using CQSplit.Core;

namespace CQSplit.Publish
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