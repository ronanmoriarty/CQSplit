using System;
using System.Threading.Tasks;
using Cafe.Waiter.Contracts;
using CQRSTutorial.Core;
using log4net;
using MassTransit;

namespace Cafe.Waiter.Service.Messaging
{
    public class OpenTabConsumer : IConsumer<IOpenTab>
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(OpenTabConsumer));
        private readonly IAggregateStore _aggregateStore;
        private readonly IEventPublisher _eventPublisher;

        public OpenTabConsumer(IAggregateStore aggregateStore, IEventPublisher eventPublisher)
        {
            _aggregateStore = aggregateStore;
            _eventPublisher = eventPublisher;
        }

        public async Task Consume(ConsumeContext<IOpenTab> context)
        {
            var message = $"Received command: Type: {typeof(IOpenTab).Name}; Command Id: {context.Message.Id}; Aggregate Id: {context.Message.AggregateId}";
            await Console.Out.WriteLineAsync(message);
            _logger.Debug(message);
        }
    }
}