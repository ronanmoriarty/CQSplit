namespace Cafe.Domain
{
    public interface IApplyEvent<in TEvent>
        where TEvent : IEvent
    {
        void Apply(TEvent @event);
    }
}
