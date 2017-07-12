namespace CQRSTutorial.Core
{
    public interface IEvent
    {
        int Id { get; set; }
        int TabId { get; set; }
    }
}