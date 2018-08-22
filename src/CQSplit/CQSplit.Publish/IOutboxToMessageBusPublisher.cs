using System.Collections.Generic;

namespace CQSplit.Publish
{
    public interface IOutboxToMessageBusPublisher
    {
        void PublishQueuedMessages();
        void PublishEvents(IEnumerable<IEvent> events);
    }
}