namespace CQ.Messaging
{
    public interface ICommandSendConfiguration
    {
        string QueueName { get; }
    }
}