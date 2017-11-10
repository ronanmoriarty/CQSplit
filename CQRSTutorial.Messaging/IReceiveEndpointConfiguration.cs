namespace CQRSTutorial.Messaging
{
    public interface IReceiveEndpointConfiguration
    {
        string QueueName { get; }
    }
}