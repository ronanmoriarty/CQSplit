namespace CQSplit.Messaging
{
    public interface IReceiveEndpointConfiguration
    {
        string QueueName { get; }
    }
}