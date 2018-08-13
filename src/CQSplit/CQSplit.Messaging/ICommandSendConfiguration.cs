namespace CQSplit.Messaging
{
    public interface ICommandSendConfiguration
    {
        string QueueName { get; }
    }
}