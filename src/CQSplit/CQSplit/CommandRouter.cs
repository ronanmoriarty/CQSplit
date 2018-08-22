using System;

namespace CQSplit
{
    public class CommandRouter : ICommandRouter
    {
        private readonly IEventHandler _eventHandler;
        private readonly ICommandHandlerProvider _commandHandlerProvider;

        public CommandRouter(IEventHandler eventHandler, ICommandHandlerProvider commandHandlerProvider)
        {
            _eventHandler = eventHandler;
            _commandHandlerProvider = commandHandlerProvider;
        }

        public void Route<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            if (command.Id == Guid.Empty)
            {
                throw new ArgumentException("Command does not have Id set.");
            }

            var handler = _commandHandlerProvider.GetCommandHandler(command);
            var events = handler.Handle(command);
            _eventHandler.Handle(events);
        }
    }
}