using System.Collections.Generic;

namespace CQ.Core
{
    public interface IEventHandler
    {
        void Handle(IEnumerable<IEvent> events);
    }
}