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

        public ICommandHandler GetCommandHandler(ICommand command)
        {
            return _commandHandlers.SingleOrDefault(x => x.CanHandle(command));
        }
    }
}