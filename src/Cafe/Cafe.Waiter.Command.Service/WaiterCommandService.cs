using CQRSTutorial.Publish;
using log4net;
using MassTransit;

namespace Cafe.Waiter.Command.Service
{
    public class WaiterCommandService
    {
        private readonly IOutboxToMessageBusPublisher _outboxToMessageBusPublisher;
        private readonly IBusControl _busControl;
        private readonly ILog _logger = LogManager.GetLogger(typeof(WaiterCommandService));

        public WaiterCommandService(IBusControl busControl, IOutboxToMessageBusPublisher outboxToMessageBusPublisher)
        {
            _outboxToMessageBusPublisher = outboxToMessageBusPublisher;
            _busControl = busControl;
        }

        public void Start()
        {
            _logger.Info("Starting service.");
            _busControl.Start();
            _outboxToMessageBusPublisher.PublishQueuedMessages();
        }

        public void Stop()
        {
            _logger.Info("Stopping service.");
            _busControl.Stop();
        }
    }
}