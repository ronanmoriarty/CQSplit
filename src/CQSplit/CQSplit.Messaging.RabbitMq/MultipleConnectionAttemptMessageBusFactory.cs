using System;
using System.Threading;
using MassTransit;
using NLog;

namespace CQSplit.Messaging.RabbitMq
{
    public class MultipleConnectionAttemptMessageBusFactory : IMessageBusFactory
    {
        private readonly IMessageBusFactory _messageBusFactory;
        private readonly IRabbitMqHostConfiguration _rabbitMqHostConfiguration;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public MultipleConnectionAttemptMessageBusFactory(IMessageBusFactory messageBusFactory, IRabbitMqHostConfiguration rabbitMqHostConfiguration)
        {
            _messageBusFactory = messageBusFactory;
            _rabbitMqHostConfiguration = rabbitMqHostConfiguration;
        }

        public IBusControl Create()
        {
            var busControl = _messageBusFactory.Create();
            var retryCount = 0;
            var retryLimit = _rabbitMqHostConfiguration.RetryLimit;
            var delayInSecondsBetweenRetries = _rabbitMqHostConfiguration.DelayInSecondsBetweenRetries;
            var started = false;
            while (retryCount < retryLimit && !started)
            {
                try
                {
                    _logger.Info("Starting bus...");
                    busControl.Start();
                    started = true;
                }
                catch (MassTransit.RabbitMqTransport.RabbitMqConnectionException)
                {
                    _logger.Error(
                        $"Failed to connect (attempt {retryCount + 1} of {retryLimit}). Will try again in {delayInSecondsBetweenRetries} seconds.");
                    Thread.Sleep(delayInSecondsBetweenRetries * 1000);
                    retryCount++;
                    busControl = _messageBusFactory.Create(); // calling Start() more than once causes a MassTransitException, so create a new instance for next iteration instead.
                }
            }

            if (!started)
            {
                var message = "Reached retry limit trying to connect to RabbitMQ. Service stopping.";
                _logger.Error(message);
                throw new Exception(message);
            }

            return busControl;
        }
    }
}