using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using log4net;

namespace CQRSTutorial.Core
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IAggregateStore _aggregateStore;
        private readonly TypeInspector _typeInspector;
        private readonly ILog _logger = LogManager.GetLogger(typeof(CommandDispatcher));

        public CommandDispatcher(IEventPublisher eventPublisher, IAggregateStore aggregateStore, TypeInspector typeInspector)
        {
            _eventPublisher = eventPublisher;
            _aggregateStore = aggregateStore;
            _typeInspector = typeInspector;
        }

        public void Dispatch(params ICommand[] commands)
        {
            EnsureAllCommandsHaveIdSet(commands);
            foreach (var command in commands)
            {
                var handler = GetCommandHandlerFor(command);
                var commandType = command.GetType();
                var handleMethod = _typeInspector.FindMethodTakingSingleArgument(handler.GetType(), GetHandleMethodName(), commandType);
                try
                {
                    var events = (IEnumerable<IEvent>)handleMethod.Invoke(handler, new[] { command });
                    _eventPublisher.Publish(events);
                }
                catch (TargetInvocationException exception)
                {
                    _logger.Error(exception.InnerException.StackTrace);
                    throw exception.InnerException; // allow any actual exceptions to bubble up, rather than wrapping up the original exception in the reflection-specific TargetInvocationException.
                }
            }
        }

        private void EnsureAllCommandsHaveIdSet(ICommand[] commands)
        {
            if (commands.Any(command => command.Id == Guid.Empty))
            {
                throw new ArgumentException("At least one command does not have Id set.");
            }
        }

        private object GetCommandHandlerFor(ICommand command)
        {
            object handler = _aggregateStore.GetCommandHandler(command);
            return handler;
        }

        private string GetHandleMethodName()
        {
            Expression<Action> objectExpression = () => ((ICommandHandler<IEvent>)null).Handle(null); // done this way instead of just returning "Handle" to facilitate any potential future refactoring / renaming.
            return ((MethodCallExpression)objectExpression.Body).Method.Name;
        }
    }
}