using MassTransit;

namespace CQRSTutorial.Messaging
{
    public interface IMessageBusFactory
    {
        IBusControl Create();
    }
}