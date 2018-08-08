using System.Collections.Generic;

namespace CQ.Core
{
    public interface ICommandHandler<in TCommand> : ICommandHandler
    {
        IEnumerable<IEvent> Handle(TCommand command);
    }

    public interface ICommandHandler
    {
        bool CanHandle(ICommand command);
    }
}