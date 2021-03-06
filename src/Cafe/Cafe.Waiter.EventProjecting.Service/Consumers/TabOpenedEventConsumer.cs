using Cafe.Waiter.EventProjecting.Service.Projectors;
using Cafe.Waiter.Events;
using CQSplit.Messaging;

namespace Cafe.Waiter.EventProjecting.Service.Consumers
{
    public class TabOpenedEventConsumer : EventConsumer<TabOpened>
    {
        public TabOpenedEventConsumer(ITabOpenedProjector projector)
            : base(projector)
        {
        }
    }
}