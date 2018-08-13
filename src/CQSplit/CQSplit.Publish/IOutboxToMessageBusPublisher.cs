using System.Collections.Generic;
using CQSplit.Core;

namespace CQSplit.Publish
{
    public interface IOutboxToMessageBusPublisher
    {
        void PublishQueuedMessages();
        void PublishEvents(IEnumerable<IEvent> events);
    }
}