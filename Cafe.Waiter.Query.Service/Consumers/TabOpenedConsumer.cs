using Cafe.Waiter.Events;
using Cafe.Waiter.Query.Service.Projectors;

namespace Cafe.Waiter.Query.Service.Consumers
{
    public class TabOpenedConsumer : Consumer<TabOpened>
    {
        public TabOpenedConsumer(ITabOpenedProjector projector)
            : base(projector)
        {
        }
    }
}