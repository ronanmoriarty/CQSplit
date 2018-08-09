using CQ.Core;

namespace CQ.DAL
{
    public interface IEventStore : IHaveUnitOfWork
    {
        void Add(IEvent @event);
    }
}