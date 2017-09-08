using Cafe.Waiter.Events;
using Cafe.Waiter.Query.Service.Projectors;

namespace Cafe.Waiter.Query.Service.Consumers
{
    public class TabOpenedConsumer : Consumer<TabOpened> // TODO remove reference to Cafe.Domain - reference constraint on IConsumer is preventing use of ITabOpened instead.
    {
        public TabOpenedConsumer(ITabOpenedProjector projector)
            : base(projector)
        {
        }
    }
}