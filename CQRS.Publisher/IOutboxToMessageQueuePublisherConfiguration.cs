namespace CQRSTutorial.Publisher
{
    public interface IOutboxToMessageQueuePublisherConfiguration
    {
        int BatchSize { get; }
        string QueueName { get; }
    }
}