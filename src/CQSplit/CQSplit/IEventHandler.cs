using System.Collections.Generic;

namespace CQSplit
{
    public interface IEventHandler
    {
        void Handle(IEnumerable<IEvent> events);
    }
}