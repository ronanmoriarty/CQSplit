namespace CQRSTutorial.Publisher
{
    public interface IOutboxToMessageQueuePublisher
    {
        void PublishQueuedMessages();
    }
}