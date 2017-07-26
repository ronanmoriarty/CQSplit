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
            return _commandHandlers.SingleOrDefault(x => x.CanHandle(command));
        }
    }
}