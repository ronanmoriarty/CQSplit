using System.Threading;
using System.Threading.Tasks;
using CQRSTutorial.Publish;
using log4net;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace Cafe.Waiter.Command.Service
{
    public class WaiterCommandService : IHostedService
    {
        private readonly IOutboxToMessageBusPublisher _outboxToMessageBusPublisher;
        private readonly IBusControl _busControl;
        private readonly ILog _logger = LogManager.GetLogger(typeof(WaiterCommandService));

        public WaiterCommandService(IBusControl busControl, IOutboxToMessageBusPublisher outboxToMessageBusPublisher)
        {
            _busControl = busControl;
            _outboxToMessageBusPublisher = outboxToMessageBusPublisher;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => Start(), cancellationToken);
        }

        public void Start()
        {
            _logger.Info("Starting service.");
            _outboxToMessageBusPublisher.PublishQueuedMessages();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => Stop(), cancellationToken);
        }

        public void Stop()
        {
            _logger.Info("Stopping service.");
            _busControl.Stop();
        }
    }
}