namespace CQRSTutorial.Core
{
    public interface IEvent
    {
        int Id { get; set; }
        int AggregateId { get; set; }
    }
}