using CQRSTutorial.Messaging;
using MassTransit;

namespace Cafe.Waiter.Query.Service
{
    public class WaiterQueryService
    {
        private readonly IMessageBusFactory _messageBusFactory;
        private IBusControl _busControl;

        public WaiterQueryService(IMessageBusFactory messageBusFactory)
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