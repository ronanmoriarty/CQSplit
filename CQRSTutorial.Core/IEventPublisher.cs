using System.Collections.Generic;

namespace CQRSTutorial.Core
{
    public interface IEventPublisher
    {
        void Publish(IEnumerable<IEvent> events);
    }
}