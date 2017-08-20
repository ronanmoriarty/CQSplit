using System;
using System.Reflection;
using log4net;

namespace CQRSTutorial.Core
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IAggregateStore _aggregateStore;
        private readonly ILog _logger = LogManager.GetLogger(typeof(CommandDispatcher));

        public CommandDispatcher(IEventPublisher eventPublisher, IAggregateStore aggregateStore)
        {
            _eventPublisher = eventPublisher;
            _aggregateStore = aggregateStore;
        }

        public void Dispatch<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            if (command.Id == Guid.Empty)
            {
                throw new ArgumentException("Command does not have Id set.");
            }

            var handler = _aggregateStore.GetCommandHandler(command);
            try
            {
                var events = handler.Handle(command);
                _eventPublisher.Publish(events);
            }
            catch (TargetInvocationException exception)
            {
                _logger.Error(exception.InnerException.StackTrace);
                throw exception.InnerException; // allow any actual exceptions to bubble up, rather than wrapping up the original exception in the reflection-specific TargetInvocationException.
            }
        }
    }
}