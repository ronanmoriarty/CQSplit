using System;
using System.Collections.Generic;
using System.Linq;

namespace CQRSTutorial.Core
{
    public class CommandHandlerProvider : ICommandHandlerProvider
    {
        private readonly IList<ICommandHandler> _commandHandlers;
        private readonly ICommandHandlerFactory _commandHandlerFactory;

        public CommandHandlerProvider(ICommandHandlerFactory commandHandlerFactory)
            : this(new List<ICommandHandler>(), commandHandlerFactory)
        {
        }

        public CommandHandlerProvider(IEnumerable<ICommandHandler> commandHandlers,
            ICommandHandlerFactory commandHandlerFactory)
        {
            _commandHandlers = commandHandlers.ToList();
            _commandHandlerFactory = commandHandlerFactory;
        }

        public ICommandHandler<TCommand> GetCommandHandler<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            ICommandHandler<TCommand> handler;
            try
            {
                handler = (ICommandHandler<TCommand>)_commandHandlers.SingleOrDefault(x => x.CanHandle(command));
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException($"More than one type found that can handle {command.GetType()} commands");
            }

            if (handler == null)
            {
                handler = _commandHandlerFactory.CreateHandlerFor(command);
            }

            if (handler == null)
            {
                throw new ArgumentException($"Could not find any handler to handle command of type {command.GetType()}");
            }

            return handler;
        }
    }
}