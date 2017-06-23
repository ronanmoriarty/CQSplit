using System;
using System.Linq;
using System.Linq.Expressions;

namespace CQRSTutorial.Core
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
            var applyEventMethodName = GetApplyEventMethodName();
            if (commandHandler != null)
            {
                var applyMethodInfo = commandHandler.FindMethodTakingSingleArgument(applyEventMethodName, eventType);
                Console.WriteLine($"Invoking {applyEventMethodName}() for {eventType.FullName}...");
                applyMethodInfo?.Invoke(commandHandler, new object[] {@event});
            }
            else
            {
                Console.WriteLine($"Could not find {applyEventMethodName}() taking argument of type {eventType.FullName}.");
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

        private string GetApplyEventMethodName()
        {
            Expression<Action> objectExpression = () => ((IApplyEvent<IEvent>)null).Apply(null); // done this way instead of just returning "Apply" to facilitate any potential future refactoring / renaming.
            return ((MethodCallExpression) objectExpression.Body).Method.Name;
        }
    }
}