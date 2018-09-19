namespace CQSplit.Messaging.RabbitMq
{
    public interface IRabbitMqHostConfiguration
    {
        string Uri { get; }
        string Username { get; }
        string Password { get; }
        int RetryLimit { get; }
        int DelayInSecondsBetweenRetries { get; }
    }
}