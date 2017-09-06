using CQRSTutorial.Core;

namespace Cafe.Waiter.Query.Service.Consumers
{
    public interface IProjector
    {
        void Project(IEvent message);
    }
}