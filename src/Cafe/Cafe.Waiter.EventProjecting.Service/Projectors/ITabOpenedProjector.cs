using Cafe.Waiter.Events;
using CQRSTutorial.Messaging;

namespace Cafe.Waiter.EventProjecting.Service.Projectors
{
    public interface ITabOpenedProjector : IProjector<TabOpened>
    {
    }
}