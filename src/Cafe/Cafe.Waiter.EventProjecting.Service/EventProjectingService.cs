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
            _busControl.Start();
        }

        public void Stop()
        {
            _logger.Info("Stopping service.");
            _busControl.Stop();
        }
    }
}