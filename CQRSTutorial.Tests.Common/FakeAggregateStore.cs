using System.Collections.Generic;
using System.Linq;
using CQRSTutorial.Core;

namespace CQRSTutorial.Tests.Common
{
    public class FakeAggregateStore : IAggregateStore
    {
        private readonly IEnumerable<ICommandHandler> _commandHandlers;

        public FakeAggregateStore(IEnumerable<ICommandHandler> commandHandlers)
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