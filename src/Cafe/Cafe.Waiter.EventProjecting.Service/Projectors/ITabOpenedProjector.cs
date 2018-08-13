using Cafe.Waiter.Events;
using CQSplit.Messaging;

namespace Cafe.Waiter.EventProjecting.Service.Projectors
{
    public interface ITabOpenedProjector : IProjector<TabOpened>
    {
    }
}