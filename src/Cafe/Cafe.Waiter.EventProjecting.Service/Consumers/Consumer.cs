using System.Threading.Tasks;
using Cafe.Waiter.EventProjecting.Service.Projectors;
using CQRSTutorial.Core;
using log4net;
using MassTransit;

namespace Cafe.Waiter.EventProjecting.Service.Consumers
{
    public abstract class Consumer<TEvent> : IConsumer<TEvent>
        where TEvent : class, IEvent
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(Consumer<>));
        private readonly IProjector<TEvent> _projector;

        protected Consumer(IProjector<TEvent> projector)
        {
            _projector = projector;
        }

        public async Task Consume(ConsumeContext<TEvent> context)
        {
            var text = $"Received event: {GetMessageDescription(context)}";
            _logger.Debug(text);
            try
            {
                _projector.Project(context.Message);
            }
            catch (System.Exception exception)
            {
                _logger.Error($"Error processing {GetMessageDescription(context)}");
                _logger.Error(exception);
                throw;
            }
        }

        private string GetMessageDescription(ConsumeContext<TEvent> context)
        {
            return $"Type: {typeof(TEvent).Name}; Event Id: {context.Message.Id}; Aggregate Id: {context.Message.AggregateId}";
        }
    }
}