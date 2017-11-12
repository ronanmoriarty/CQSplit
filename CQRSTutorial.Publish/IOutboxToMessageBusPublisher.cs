namespace CQRSTutorial.Publish
{
    public interface IOutboxToMessageBusPublisher
    {
        void PublishQueuedMessages();
    }
}