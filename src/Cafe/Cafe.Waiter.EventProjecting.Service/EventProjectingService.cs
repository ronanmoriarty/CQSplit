using System.Threading;
using System.Threading.Tasks;
using CQRSTutorial.Messaging;
using MassTransit;
using Microsoft.Extensions.Hosting;
using NLog;

namespace Cafe.Waiter.EventProjecting.Service
{
    public class EventProjectingService : IHostedService
    {
        private readonly IMessageBusFactory _messageBusFactory;
        private IBusControl _busControl;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public EventProjectingService(IMessageBusFactory messageBusFactory)
        {
            _messageBusFactory = messageBusFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Run(() => Start(), cancellationToken);
        }

        public void Start()
        {
            _logger.Info("Starting service.");
            _busControl = new MultipleConnectionAttemptMessageBusFactory(_messageBusFactory).Create();
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