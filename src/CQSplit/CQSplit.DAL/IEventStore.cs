using CQSplit.Core;

namespace CQSplit.DAL
{
    public interface IEventStore : IHaveUnitOfWork
    {
        void Add(IEvent @event);
    }
}