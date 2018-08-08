using MassTransit;

namespace CQ.Messaging
{
    public interface IInMemoryReceiveEndpointsConfigurator
    {
        void Configure(IInMemoryBusFactoryConfigurator inMemoryBusFactoryConfigurator);
    }
}