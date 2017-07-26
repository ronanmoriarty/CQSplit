using System;
using System.Collections.Generic;
using System.Linq;

namespace CQRSTutorial.Core
{
    public class AggregateStore : IAggregateStore
    {
        private readonly IEnumerable<ICommandHandler> _commandHandlers;

        public AggregateStore(IEnumerable<ICommandHandler> commandHandlers)
        {
            _commandHandlers = commandHandlers;
        }

        public ICommandHandler GetCommandHandler(ICommand command)
        {
            ICommandHandler handler;
            try
            {
                handler = _commandHandlers.SingleOrDefault(x => x.CanHandle(command));
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException($"More than one type found that can handle {command.GetType()} commands");
            }

            if (handler == null)
            {
                throw new ArgumentException($"Could not find any handler to handle command of type {command.GetType()}");
            }

            return handler;
        }
    }
}