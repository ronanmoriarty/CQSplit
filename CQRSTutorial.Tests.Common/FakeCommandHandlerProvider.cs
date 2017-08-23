using System.Collections.Generic;
using System.Linq;
using CQRSTutorial.Core;

namespace CQRSTutorial.Tests.Common
{
    public class FakeCommandHandlerProvider : ICommandHandlerProvider
    {
        private readonly IList<ICommandHandler> _commandHandlers = new List<ICommandHandler>();

        public ICommandHandler<TCommand> GetCommandHandler<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            return (ICommandHandler<TCommand>)_commandHandlers.SingleOrDefault(x => x.CanHandle(command));
        }

        public void RegisterCommandHandler(ICommandHandler commandHandler)
        {
            _commandHandlers.Add(commandHandler);
        }
    }
}