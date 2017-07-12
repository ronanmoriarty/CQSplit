using System;
using System.Linq;
using Cafe.Domain.Events;
using CQRSTutorial.Core;
using Newtonsoft.Json;

namespace CQRSTutorial.DAL
{
    public class EventMapper
    {
        public IEvent MapToEvent(Event storedEvent)
        {
            var @event = (IEvent)JsonConvert.DeserializeObject(storedEvent.Data, GetEventTypeFrom(storedEvent.EventType));
            AssignEventIdFromEventToPublishId(@event, storedEvent);
            return @event;
        }

        private Type GetEventTypeFrom(string eventType)
        {
            return typeof(TabOpened).Assembly.GetTypes()
                .Single(t => typeof(IEvent).IsAssignableFrom(t) && t.Name == eventType);
        }

        private void AssignEventIdFromEventToPublishId(IEvent @event, Event eventToPublish)
        {
            @event.Id = eventToPublish.Id;
        }
    }
}