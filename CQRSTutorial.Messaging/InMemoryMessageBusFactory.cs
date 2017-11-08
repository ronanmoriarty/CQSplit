using MassTransit;

namespace CQRSTutorial.Messaging
{
    public class InMemoryMessageBusFactory : IMessageBusFactory
    {
        private readonly IInMemoryReceiveEndpointsConfigurator _inMemoryReceiveEndpointsConfigurator;

        public InMemoryMessageBusFactory(IInMemoryReceiveEndpointsConfigurator inMemoryReceiveEndpointsConfigurator)
        {
            _inMemoryReceiveEndpointsConfigurator = inMemoryReceiveEndpointsConfigurator;
        }

        public IBusControl Create()
        {
            return Bus.Factory.CreateUsingInMemory(ConfigureReceiveEndpoints);
        }

        private void ConfigureReceiveEndpoints(IInMemoryBusFactoryConfigurator inMemoryBusFactoryConfigurator)
        {
            _inMemoryReceiveEndpointsConfigurator.Configure(inMemoryBusFactoryConfigurator);
        }
    }
}