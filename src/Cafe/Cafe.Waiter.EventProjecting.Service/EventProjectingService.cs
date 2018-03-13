using CQRSTutorial.Messaging;
using MassTransit;

namespace Cafe.Waiter.EventProjecting.Service
{
    public class EventProjectingService
    {
        private readonly IMessageBusFactory _messageBusFactory;
        private IBusControl _busControl;

        public EventProjectingService(IMessageBusFactory messageBusFactory)
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