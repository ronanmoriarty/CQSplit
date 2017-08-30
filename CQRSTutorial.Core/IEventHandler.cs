using System.Collections.Generic;

namespace CQRSTutorial.Core
{
    public interface IEventHandler
    {
        void Handle(IEnumerable<IEvent> events);
    }
}