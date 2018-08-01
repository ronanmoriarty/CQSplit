using System.Threading;
using System.Threading.Tasks;
using CQRSTutorial.Publish;
using MassTransit;
using Microsoft.Extensions.Hosting;
using NLog;

namespace Cafe.Waiter.Command.Service
{
    public class WaiterCommandService : IHostedService
    {
        private readonly IOutboxToMessageBusPublisher _outboxToMessageBusPublisher;
        private readonly IBusControl _busControl;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

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