using System.Collections.Generic;
using System.Linq;

namespace CQRSTutorial.Core
{
    public class CompositeEventHandler : IEventHandler
    {
        private readonly IEnumerable<IEventHandler> _eventHandlers;

        public CompositeEventHandler(IEnumerable<IEventHandler> eventHandlers)
        {
            _eventHandlers = eventHandlers;
        }

        public void Handle(IEnumerable<IEvent> events)
        {
            var listOfEvents = events as IList<IEvent> ?? events.ToList();
            foreach (var eventHandler in _eventHandlers)
            {
                eventHandler.Handle(listOfEvents);
            }
        }
    }
}
