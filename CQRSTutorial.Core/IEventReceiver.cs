using System.Collections.Generic;

namespace CQRSTutorial.Core
{
    public interface IEventReceiver
    {
        void Receive(IEnumerable<IEvent> events);
    }
}