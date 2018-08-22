using System;
using System.Linq.Expressions;
using NLog;

namespace CQSplit
{
    public class EventApplier : IEventApplier
    {
        private readonly TypeInspector _typeInspector;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public EventApplier(TypeInspector typeInspector)
        {
            _typeInspector = typeInspector;
        }

        public void ApplyEvent(IEvent @event, object eventHandler)
        {
            var eventType = @event.GetType();
            var applyEventMethodName = GetApplyEventMethodName();
            var applyMethodInfo = _typeInspector.FindMethodTakingSingleArgument(eventHandler.GetType(), applyEventMethodName, eventType);
            _logger.Debug($"Invoking {applyEventMethodName}() for {eventType.FullName}...");
            applyMethodInfo?.Invoke(eventHandler, new object[] { @event });
        }

        private string GetApplyEventMethodName()
        {
            Expression<Action> objectExpression = () => ((IApplyEvent<IEvent>)null).Apply(null); // done this way instead of just returning "Apply" to facilitate any potential future refactoring / renaming.
            return ((MethodCallExpression) objectExpression.Body).Method.Name;
        }
    }
}