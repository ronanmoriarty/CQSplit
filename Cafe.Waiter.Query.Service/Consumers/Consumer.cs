using System.Threading.Tasks;
using Cafe.Waiter.Query.Service.Projectors;
using CQRSTutorial.Core;
using log4net;
using MassTransit;

namespace Cafe.Waiter.Query.Service.Consumers
{
    public abstract class Consumer<TEvent> : IConsumer<TEvent>
        where TEvent : class, IEvent
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(Consumer<>));
        private IProjector<TEvent> _projector;

        protected Consumer(IProjector<TEvent> projector)
        {
            _projector = projector;
        }

        public async Task Consume(ConsumeContext<TEvent> context)
        {
            var text = $"Received event: Type: {typeof(TEvent).Name}; Event Id: {context.Message.Id}; Aggregate Id: {context.Message.AggregateId}";
            _logger.Debug(text);
            _projector.Project(context.Message);
        }
    }
}