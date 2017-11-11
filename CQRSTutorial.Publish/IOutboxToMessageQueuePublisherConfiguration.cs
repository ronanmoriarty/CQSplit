namespace CQRSTutorial.Publish
{
    public interface IOutboxToMessageQueuePublisherConfiguration
    {
        int BatchSize { get; }
    }
}