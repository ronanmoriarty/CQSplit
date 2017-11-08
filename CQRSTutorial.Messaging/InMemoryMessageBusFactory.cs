using MassTransit;

namespace CQRSTutorial.Messaging
{
    public class InMemoryMessageBusFactory : IMessageBusFactory
    {
        private readonly IInMemoryReceiveEndpointsConfigurator[] _inMemoryReceiveEndpointsConfigurators;

        public InMemoryMessageBusFactory(params IInMemoryReceiveEndpointsConfigurator[] inMemoryReceiveEndpointsConfigurators)
        {
            _inMemoryReceiveEndpointsConfigurators = inMemoryReceiveEndpointsConfigurators;
        }

        public IBusControl Create()
        {
            return Bus.Factory.CreateUsingInMemory(ConfigureReceiveEndpoints);
        }

        private void ConfigureReceiveEndpoints(IInMemoryBusFactoryConfigurator inMemoryBusFactoryConfigurator)
        {
            foreach (var inMemoryReceiveEndpointsConfigurator in _inMemoryReceiveEndpointsConfigurators)
            {
                inMemoryReceiveEndpointsConfigurator.Configure(inMemoryBusFactoryConfigurator);
            }
        }
    }
}