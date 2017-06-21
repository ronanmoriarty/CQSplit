using System;
using System.Linq;
using System.Linq.Expressions;
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

        public void ApplyEvent(IEvent @event)
        {
            var eventType = @event.GetType();
            var commandHandler = _commandHandlers.SingleOrDefault(CanApplyEventOfType(eventType));
            if (commandHandler != null)
            {
                var applyMethodInfo = FindApplyEventOverloadFor(eventType, commandHandler);
                Console.WriteLine($"Invoking {GetApplyEventMethodName()}() for {eventType.FullName}...");
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
            var applyEventMethodName = GetApplyEventMethodName();
            var applyMethodInfo = commandHandler
                .GetType()
                .GetMethods()
                .SingleOrDefault(methodInfo => methodInfo.Name == applyEventMethodName
                                               && methodInfo.GetParameters().Length == 1
                                               && methodInfo.GetParameters().Single().ParameterType == eventType);
            return applyMethodInfo;
        }

        private string GetApplyEventMethodName()
        {
            Expression<Action> objectExpression = () => ((IApplyEvent<IEvent>)null).Apply(null); // done this way instead of just returning "Apply" to facilitate any potential future refactoring / renaming.
            return ((MethodCallExpression) objectExpression.Body).Method.Name;
        }
    }
}