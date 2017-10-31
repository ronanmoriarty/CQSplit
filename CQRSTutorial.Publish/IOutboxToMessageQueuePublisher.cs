namespace CQRSTutorial.Publish
{
    public interface IOutboxToMessageQueuePublisher
    {
        void PublishQueuedMessages();
    }
}