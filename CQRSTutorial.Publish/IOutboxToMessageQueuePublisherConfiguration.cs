namespace CQRSTutorial.Publish
{
    public interface IOutboxToMessageQueuePublisherConfiguration
    {
        int BatchSize { get; }
        string QueueName { get; }
        string QueryToWatch { get; }
        string ServiceName { get; }
    }
}