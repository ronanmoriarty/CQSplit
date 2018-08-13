using MassTransit;

namespace CQSplit.Messaging
{
    public interface IMessageBusFactory
    {
        IBusControl Create();
    }
}