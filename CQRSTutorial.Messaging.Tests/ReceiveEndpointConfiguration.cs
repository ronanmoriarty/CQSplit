namespace CQRSTutorial.Messaging.Tests
{
    public class ReceiveEndpointConfiguration : IReceiveEndpointConfiguration
    {
        public ReceiveEndpointConfiguration(string queueName)
        {
            QueueName = queueName;
        }

        public string QueueName { get; }
    }
}