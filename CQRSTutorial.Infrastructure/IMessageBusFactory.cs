using MassTransit;

namespace CQRSTutorial.Infrastructure
{
    public interface IMessageBusFactory
    {
        IBusControl Create();
    }
}