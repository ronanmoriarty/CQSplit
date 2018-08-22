using System.Collections.Generic;

namespace CQSplit.Core
{
    public interface IEventHandler
    {
        void Handle(IEnumerable<IEvent> events);
    }
}