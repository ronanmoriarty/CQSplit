using System;
using System.Linq;
using System.Linq.Expressions;

namespace CQRSTutorial.Core
{
    public class EventApplier
    {
        public void ApplyEvent(IEvent @event, object[] eventHandlers)
        {
            var eventType = @event.GetType();
            var eventHandler = eventHandlers.SingleOrDefault(CanApplyEventOfType(eventType));
            var applyEventMethodName = GetApplyEventMethodName();
            if (eventHandler != null)
            {
                var applyMethodInfo = eventHandler.FindMethodTakingSingleArgument(applyEventMethodName, eventType);
                Console.WriteLine($"Invoking {applyEventMethodName}() for {eventType.FullName}...");
                applyMethodInfo?.Invoke(eventHandler, new object[] {@event});
            }
            else
            {
                Console.WriteLine($"Could not find any registered aggregates with {applyEventMethodName}() taking argument of type {eventType.FullName}.");
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