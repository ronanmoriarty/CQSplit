using CQRSTutorial.Infrastructure;
using MassTransit;

namespace Cafe.Waiter.Command.Service
{
    public class WaiterService
    {
        private readonly IMessageBusFactory _messageBusFactory;
        private IBusControl _busControl;

        public WaiterService(IMessageBusFactory messageBusFactory)
        {
            _messageBusFactory = messageBusFactory;
        }

        public void Start()
        {
            _busControl = _messageBusFactory.Create();
            _busControl.Start();
        }

        public void Stop()
        {
            _busControl.Stop();
        }
    }
}