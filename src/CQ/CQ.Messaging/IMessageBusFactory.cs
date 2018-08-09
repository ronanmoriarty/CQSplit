using MassTransit;

namespace CQ.Messaging
{
    public interface IMessageBusFactory
    {
        IBusControl Create();
    }
}