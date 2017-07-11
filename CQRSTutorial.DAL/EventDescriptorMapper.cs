using System;
using System.Linq;
using Cafe.Domain.Events;
using CQRSTutorial.Core;
using Newtonsoft.Json;

namespace CQRSTutorial.DAL
{
    public class EventDescriptorMapper
    {
        public IEvent MapEventDescriptorToEvent(EventDescriptor eventDescriptor)
        {
            var @event = (IEvent)JsonConvert.DeserializeObject(eventDescriptor.Data, GetEventTypeFrom(eventDescriptor.EventType));
            AssignEventIdFromEventDescriptorId(@event, eventDescriptor);
            return @event;
        }

        private Type GetEventTypeFrom(string eventType)
        {
            return typeof(TabOpened).Assembly.GetTypes()
                .Single(t => typeof(ITabEvent).IsAssignableFrom(t) && t.Name == eventType);
        }

        private void AssignEventIdFromEventDescriptorId(IEvent @event, EventDescriptor eventDescriptor)
        {
            @event.Id = eventDescriptor.Id;
        }
    }
}