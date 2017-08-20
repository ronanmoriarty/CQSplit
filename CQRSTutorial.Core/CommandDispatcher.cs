using System;
using System.Linq.Expressions;
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
            EnsureCommandHasIdSet(command);
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

        private void EnsureCommandHasIdSet(ICommand command)
        {
            if (command.Id == Guid.Empty)
            {
                throw new ArgumentException("Command does not have Id set.");
            }
        }

        private string GetHandleMethodName()
        {
            Expression<Action> objectExpression = () => ((ICommandHandler<IEvent>)null).Handle(null); // done this way instead of just returning "Handle" to facilitate any potential future refactoring / renaming.
            return ((MethodCallExpression)objectExpression.Body).Method.Name;
        }
    }
}