namespace CQSplit.Core
{
    public interface IEventApplier
    {
        void ApplyEvent(IEvent @event, object eventHandler);
    }
}