using System;

namespace CQRSTutorial.Core
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IAggregateStore _aggregateStore;

        public CommandDispatcher(IEventPublisher eventPublisher, IAggregateStore aggregateStore)
        {
            _eventPublisher = eventPublisher;
            _aggregateStore = aggregateStore;
        }

        public void Dispatch<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            if (command.Id == Guid.Empty)
            {
                throw new ArgumentException("Command does not have Id set.");
            }

            var handler = _aggregateStore.GetCommandHandler(command);
            var events = handler.Handle(command);
            _eventPublisher.Publish(events);
        }
    }
}