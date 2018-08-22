namespace CQSplit.Serialization
{
    public interface IEventStore : IHaveUnitOfWork
    {
        void Add(IEvent @event);
    }
}