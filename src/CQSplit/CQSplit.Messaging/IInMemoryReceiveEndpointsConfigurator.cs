using MassTransit;

namespace CQSplit.Messaging
{
    public interface IInMemoryReceiveEndpointsConfigurator
    {
        void Configure(IInMemoryBusFactoryConfigurator inMemoryBusFactoryConfigurator);
    }
}