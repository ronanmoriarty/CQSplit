using Cafe.Waiter.Contracts;
using CQRSTutorial.Infrastructure;
using log4net;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace Cafe.Waiter.Service
{
    public class WaiterService
    {
        private readonly IMessageBusFactory _messageBusFactory;
        private readonly ILog _logger = LogManager.GetLogger(typeof(WaiterService));
        private readonly IServiceAddressProvider _serviceAddressProvider;
        private IBusControl _busControl;

        public WaiterService(IMessageBusFactory messageBusFactory, IServiceAddressProvider serviceAddressProvider)
        {
            _messageBusFactory = messageBusFactory;
            _serviceAddressProvider = serviceAddressProvider;
        }

        public void Start()
        {
            _busControl = _messageBusFactory.Create(Configure);
            _busControl.Start();
        }

        private void Configure(IRabbitMqBusFactoryConfigurator configurator, IRabbitMqHost host)
        {
            var serviceAddress = _serviceAddressProvider.GetServiceAddressFor<IOpenTab>();
            _logger.Debug($"Endpoint for IOpenTab is \"{serviceAddress}\"");
            configurator.ReceiveEndpoint(host, serviceAddress, endpointConfigurator =>
            {
                endpointConfigurator.Consumer<OpenTabCommandHandler>();
            });
        }

        public void Stop()
        {
            _busControl.Stop();
        }
    }
}