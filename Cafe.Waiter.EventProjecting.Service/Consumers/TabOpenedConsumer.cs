using Cafe.Waiter.EventProjecting.Service.Projectors;
using Cafe.Waiter.Events;

namespace Cafe.Waiter.EventProjecting.Service.Consumers
{
    public class TabOpenedConsumer : Consumer<TabOpened>
    {
        public TabOpenedConsumer(ITabOpenedProjector projector)
            : base(projector)
        {
        }
    }
}