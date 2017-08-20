using System.Collections.Generic;
using System.Linq;
using CQRSTutorial.Core;

namespace CQRSTutorial.Tests.Common
{
    public class FakeCommandHandlerProvider : ICommandHandlerProvider
    {
        private readonly IEnumerable<ICommandHandler> _commandHandlers;

        public FakeCommandHandlerProvider(IEnumerable<ICommandHandler> commandHandlers)
        {
            _commandHandlers = commandHandlers;
        }

        public ICommandHandler<TCommand> GetCommandHandler<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            return (ICommandHandler<TCommand>)_commandHandlers.SingleOrDefault(x => x.CanHandle(command));
        }
    }
}