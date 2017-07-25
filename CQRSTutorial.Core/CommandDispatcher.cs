using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CQRSTutorial.Core
{
    public class CommandDispatcher
    {
        private readonly IEventReceiver _eventReceiver;
        private readonly ICommandHandler[] _commandHandlers;
        private Dictionary<Type, Type> _commandHandlerMappings;
        private readonly TypeInspector _typeInspector;

        public CommandDispatcher(IEventReceiver eventReceiver, ICommandHandler[] commandHandlers, TypeInspector typeInspector, Assembly assemblyContainingCommandHandlers)
        {
            _eventReceiver = eventReceiver;
            _commandHandlers = commandHandlers;
            MapCommandTypesToCommandHandlerTypes(assemblyContainingCommandHandlers);
            _typeInspector = typeInspector;
        }

        public void Dispatch(params ICommand[] commands)
        {
            foreach (var command in commands)
            {
                var commandType = command.GetType();
                var handlerType = _commandHandlerMappings[commandType];
                var handler = _commandHandlers
                    .Where(commandHandler => commandHandler.GetType() == handlerType)
                    .SingleOrDefault(x => x.Id == command.AggregateId);
                if (handler == null)
                {
                    handler = _commandHandlers.Single(commandHandler => commandHandler.GetType() == handlerType); // e.g. TabFactory
                }
                if (handler == null)
                {
                    throw new ArgumentException("handler");
                }

                var handleMethod = _typeInspector.FindMethodTakingSingleArgument(handlerType, GetHandleMethodName(), commandType);
                try
                {
                    var events = (IEnumerable<IEvent>)handleMethod.Invoke(handler, new[] { command });
                    _eventReceiver.Receive(events);
                }
                catch (TargetInvocationException exception)
                {
                    Console.WriteLine(exception.InnerException.StackTrace);
                    throw exception.InnerException; // allow any actual exceptions to bubble up, rather than wrapping up the original exception in the reflection-specific TargetInvocationException.
                }
            }
        }

        private string GetHandleMethodName()
        {
            Expression<Action> objectExpression = () => ((ICommandHandler<IEvent>)null).Handle(null); // done this way instead of just returning "Handle" to facilitate any potential future refactoring / renaming.
            return ((MethodCallExpression)objectExpression.Body).Method.Name;
        }

        private void MapCommandTypesToCommandHandlerTypes(Assembly assemblyContainingCommandHandlers)
        {
            _commandHandlerMappings = new Dictionary<Type, Type>();
            foreach (var type in assemblyContainingCommandHandlers.GetTypes())
            {
                foreach (var interfaceType in type.GetInterfaces())
                {
                    if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
                    {
                        var commandType = interfaceType.GenericTypeArguments[0];
                        if (_commandHandlerMappings.ContainsKey(commandType))
                        {
                            throw new ArgumentException($"More than one type found that can handle {commandType.FullName} commands");
                        }

                        _commandHandlerMappings.Add(commandType, type);
                    }
                }
            }
        }
    }
}