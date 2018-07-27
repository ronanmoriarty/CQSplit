using System;
using System.Threading;
using CQRSTutorial.Messaging;
using log4net;
using MassTransit;

namespace Cafe.Waiter.EventProjecting.Service
{
    public class EventProjectingService
    {
        private readonly IMessageBusFactory _messageBusFactory;
        private IBusControl _busControl;
        private readonly ILog _logger = LogManager.GetLogger(typeof(EventProjectingService));

        public EventProjectingService(IMessageBusFactory messageBusFactory)
        {
            _messageBusFactory = messageBusFactory;
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

        public void Stop()
        {
            _logger.Info("Stopping service.");
            _busControl.Stop();
        }
    }
}