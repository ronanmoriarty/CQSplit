using System.Collections.Generic;

namespace Cafe.Domain
{
    public interface IEventPublisher
    {
        void Publish(IEnumerable<IEvent> events);
    }
}