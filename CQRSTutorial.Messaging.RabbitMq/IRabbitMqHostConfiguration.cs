namespace CQRSTutorial.Messaging.RabbitMq
{
    public interface IRabbitMqHostConfiguration
    {
        string Uri { get; }
        string Username { get; }
        string Password { get; }
    }
}