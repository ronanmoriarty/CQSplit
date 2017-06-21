using System;
using System.Linq;
using System.Reflection;

namespace Cafe.Domain
{
    public class EventApplier
    {
        private readonly object[] _commandHandlers;

        public EventApplier(object[] commandHandlers)
        {
            _commandHandlers = commandHandlers;
        }

        public void ApplyEvent(Type eventType, IEvent @event)
        {
            var commandHandler = _commandHandlers.SingleOrDefault(CanApplyEventOfType(eventType));
            if (commandHandler != null)
            {
                var applyMethodInfo = FindApplyEventOverloadFor(eventType, commandHandler);
                Console.WriteLine($"Invoking Apply() for {eventType.FullName}...");
                applyMethodInfo?.Invoke(commandHandler, new object[] {@event});
            }
        }

        private Func<object, bool> CanApplyEventOfType(Type eventType)
        {
            return commandHandler => commandHandler
                .GetType()
                .GetInterfaces()
                .Any(interfaceType =>
                    interfaceType.IsGenericType
                    && interfaceType.GetGenericTypeDefinition() == typeof(IApplyEvent<>)
                    && interfaceType.GenericTypeArguments.Single() == eventType
                );
        }

        private MethodInfo FindApplyEventOverloadFor(Type eventType, object commandHandler)
        {
            var applyMethodInfo = commandHandler
                .GetType()
                .GetMethods()
                .SingleOrDefault(methodInfo => methodInfo.Name == "Apply" // TODO use expression to make refactoring / renaming methods easier.
                                               && methodInfo.GetParameters().Length == 1
                                               && methodInfo.GetParameters().Single().ParameterType == eventType);
            return applyMethodInfo;
        }
    }
}