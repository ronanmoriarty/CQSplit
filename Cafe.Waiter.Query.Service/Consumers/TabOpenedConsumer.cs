using Cafe.Domain.Events;

namespace Cafe.Waiter.Query.Service.Consumers
{
    public class TabOpenedConsumer : Consumer<TabOpened> // TODO remove reference to Cafe.Domain - reference constraint on IConsumer is preventing use of ITabOpened instead.
    {
        public TabOpenedConsumer(IProjector projector)
            : base(projector)
        {
        }
    }
}