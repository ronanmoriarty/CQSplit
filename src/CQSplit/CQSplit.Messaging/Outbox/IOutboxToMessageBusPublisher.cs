using System.Collections.Generic;

namespace CQSplit.Messaging.Outbox
{
    public interface IOutboxToMessageBusPublisher
    {
        void PublishQueuedMessages();
        void PublishEvents(IEnumerable<IEvent> events);
    }
}