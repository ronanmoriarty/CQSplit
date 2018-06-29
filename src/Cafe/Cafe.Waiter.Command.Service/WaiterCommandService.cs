using CQRSTutorial.Publish;
using MassTransit;

namespace Cafe.Waiter.Command.Service
{
    public class WaiterCommandService
    {
        private readonly IOutboxToMessageBusPublisher _outboxToMessageBusPublisher;
        private readonly IBusControl _busControl;

        public WaiterCommandService(IBusControl busControl, IOutboxToMessageBusPublisher outboxToMessageBusPublisher)
        {
            _outboxToMessageBusPublisher = outboxToMessageBusPublisher;
            _busControl = busControl;
        }

        public void Start()
        {
            _busControl.Start();
            _outboxToMessageBusPublisher.PublishQueuedMessages();
        }

        public void Stop()
        {
            _busControl.Stop();
        }
    }
}