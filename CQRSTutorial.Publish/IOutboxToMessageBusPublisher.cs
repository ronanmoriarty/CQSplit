using System.Collections.Generic;
using CQRSTutorial.Core;

namespace CQRSTutorial.Publish
{
    public interface IOutboxToMessageBusPublisher
    {
        void PublishQueuedMessages();
        void PublishEvents(IEnumerable<IEvent> events);
    }
}