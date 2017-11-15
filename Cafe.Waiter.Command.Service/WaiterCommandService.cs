using CQRSTutorial.Messaging;
using CQRSTutorial.Publish;
using MassTransit;

namespace Cafe.Waiter.Command.Service
{
    public class WaiterCommandService
    {
        private readonly IMessageBusFactory _messageBusFactory;
        private readonly IOutboxToMessageBusPublisher _outboxToMessageBusPublisher;
        private IBusControl _busControl;

        public WaiterCommandService(IMessageBusFactory messageBusFactory,
            IOutboxToMessageBusPublisher outboxToMessageBusPublisher)
        {
            _messageBusFactory = messageBusFactory;
            _outboxToMessageBusPublisher = outboxToMessageBusPublisher;
        }

        public void Start()
        {
            _busControl = _messageBusFactory.Create();
            _busControl.Start();
            _outboxToMessageBusPublisher.PublishQueuedMessages();
        }

        public void Stop()
        {
            _busControl.Stop();
        }
    }
}