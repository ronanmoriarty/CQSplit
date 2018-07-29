using System;
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
            _busControl = _messageBusFactory.Create();
            var retryCount = 0;
            const int retryLimit = 12;
            const int delayInSecondsBetweenRetries = 10;
            var started = false;
            while (retryCount < retryLimit && !started)
            {
                try
                {
                    _logger.Info("Starting bus...");
                    _busControl.Start();
                    started = true;
                }
                catch (MassTransit.RabbitMqTransport.RabbitMqConnectionException)
                {
                    _logger.Error($"Failed to connect (attempt {retryCount + 1} of {retryLimit}). Will try again in {delayInSecondsBetweenRetries} seconds.");
                    Thread.Sleep(delayInSecondsBetweenRetries * 1000);
                    retryCount++;
                    _busControl = _messageBusFactory.Create(); // calling Start() more than once causes a MassTransitException, so create a new instance for next iteration instead.
                }
            }

            if (!started)
            {
                var message = "Reached retry limit trying to connect to RabbitMQ. Service stopping.";
                _logger.Error(message);
                throw new Exception(message);
            }
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