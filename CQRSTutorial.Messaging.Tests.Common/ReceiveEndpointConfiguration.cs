namespace CQRSTutorial.Messaging.Tests.Common
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