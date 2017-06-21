using System;
using System.Linq;
using System.Reflection;

namespace Cafe.Domain
{
    public class EventApplier
    {
        private readonly object _commandHandler;

        public EventApplier(object commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public bool CanApplyEvent(Type eventType)
        {
            var canApplyEvent = _commandHandler.GetType()
                .GetInterfaces()
                .Any(interfaceType =>
                    interfaceType.IsGenericType
                    && interfaceType.GetGenericTypeDefinition() == typeof(IApplyEvent<>)
                    && interfaceType.GenericTypeArguments.Single() == eventType
                );
            return canApplyEvent;
        }

        public void ApplyEvent(Type eventType, IEvent @event)
        {
            var applyMethodInfo = FindApplyEventOverloadFor(eventType);
            Console.WriteLine($"Invoking Apply() for {eventType.FullName}...");
            applyMethodInfo?.Invoke(_commandHandler, new object[] { @event });
        }

        private MethodInfo FindApplyEventOverloadFor(Type eventType)
        {
            var applyMethodInfo = _commandHandler
                .GetType()
                .GetMethods()
                .SingleOrDefault(methodInfo => methodInfo.Name == "Apply" // TODO use expression to make refactoring / renaming methods easier.
                                               && methodInfo.GetParameters().Length == 1
                                               && methodInfo.GetParameters().Single().ParameterType == eventType);
            return applyMethodInfo;
        }
    }
}
