using System;

namespace CQRSTutorial.Core
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly ICommandHandlerProvider _commandHandlerProvider;

        public CommandDispatcher(IEventPublisher eventPublisher, ICommandHandlerProvider commandHandlerProvider)
        {
            _eventPublisher = eventPublisher;
            _commandHandlerProvider = commandHandlerProvider;
        }

        public void Dispatch<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            if (command.Id == Guid.Empty)
            {
                throw new ArgumentException("Command does not have Id set.");
            }

            var handler = _commandHandlerProvider.GetCommandHandler(command);
            var events = handler.Handle(command);
            _eventPublisher.Publish(events);
        }
    }
}