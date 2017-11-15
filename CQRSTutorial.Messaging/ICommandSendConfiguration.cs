namespace CQRSTutorial.Messaging
{
    public interface ICommandSendConfiguration
    {
        string QueueName { get; }
    }
}