﻿using System;
using System.Threading.Tasks;
using MassTransit;
using NLog;

namespace CQSplit.Messaging
{
    public abstract class EventConsumer<TEvent> : IConsumer<TEvent>
        where TEvent : class, IEvent
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IProjector<TEvent> _projector;

        protected EventConsumer(IProjector<TEvent> projector)
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
            catch (Exception exception)
            {
                _logger.Error($"Error processing {GetMessageDescription(context)}");
                _logger.Error(exception);
                throw;
            }
        }

        private string GetMessageDescription(ConsumeContext<TEvent> context)
        {
            return $"Type: {typeof(TEvent).Name}; Event Id: {context.Message.Id}; Command Id: {context.Message.CommandId}; Aggregate Id: {context.Message.AggregateId}";
        }
    }
}