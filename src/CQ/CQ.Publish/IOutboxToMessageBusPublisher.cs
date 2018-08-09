using System.Collections.Generic;
using CQ.Core;

namespace CQ.Publish
{
    public interface IOutboxToMessageBusPublisher
    {
        void PublishQueuedMessages();
        void PublishEvents(IEnumerable<IEvent> events);
    }
}