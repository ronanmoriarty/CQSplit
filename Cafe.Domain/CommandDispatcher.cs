using System;

namespace Cafe.Domain
{
    public class CommandDispatcher
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly object[] _commandHandlers;

        public CommandDispatcher(IEventPublisher eventPublisher, object[] commandHandlers)
        {
            _eventPublisher = eventPublisher;
            _commandHandlers = commandHandlers;
        }

        public void Dispatch<TCommand>(TCommand command)
        {
            var handler = GetHandlerFor<TCommand>();
            var events = handler.Handle(command);
            _eventPublisher.Publish(events);
        }

        private ICommandHandler<TCommand> GetHandlerFor<TCommand>()
        {
            foreach (var commandHander in _commandHandlers)
            {
                var handler = commandHander as ICommandHandler<TCommand>;
                if (handler != null)
                {
                    return handler;
                }
            }

            throw new Exception($"Could not find handler for {typeof(TCommand).FullName} command.");
        }
    }
}
