using MassTransit;

namespace CQRSTutorial.Messaging
{
    public interface IInMemoryReceiveEndpointsConfigurator
    {
        void Configure(IInMemoryBusFactoryConfigurator inMemoryBusFactoryConfigurator);
    }
}