namespace CQRSTutorial.Publisher
{
    public interface IOutboxToMessageQueuePublisherConfiguration
    {
        int BatchSize { get; }
    }
}