using System.Collections.Generic;

namespace Cafe.Domain
{
    public interface ICommandHandler<in TCommand>
    {
        IEnumerable<Event> Handle(TCommand command);
    }
}