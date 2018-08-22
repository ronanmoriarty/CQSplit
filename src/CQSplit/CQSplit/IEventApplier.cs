namespace CQSplit
{
    public interface IEventApplier
    {
        void ApplyEvent(IEvent @event, object eventHandler);
    }
}