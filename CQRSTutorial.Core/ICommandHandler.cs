using System.Collections.Generic;

namespace CQRSTutorial.Core
{
    public interface ICommandHandler<in TCommand>
    {
        IEnumerable<IEvent> Handle(TCommand command);
    }
}