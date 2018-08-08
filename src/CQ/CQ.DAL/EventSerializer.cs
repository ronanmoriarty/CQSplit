using System;
using System.Linq;
using System.Reflection;
using CQ.Core;
using CQ.DAL.Serialized;
using Newtonsoft.Json;

namespace CQ.DAL
{
    public class EventSerializer
    {
        private readonly Assembly _assemblyContainingEvents;

        public EventSerializer(Assembly assemblyContainingEvents)
        {
            _assemblyContainingEvents = assemblyContainingEvents;
        }

        public Event Serialize(IEvent @event)
        {
            return new Event
            {
                Id = @event.Id,
                AggregateId = @event.AggregateId,
                CommandId = @event.CommandId,
                EventType = @event.GetType().FullName,
                Data = JsonConvert.SerializeObject(@event),
                Created = DateTime.Now
            };
        }

        public IEvent Deserialize(Event serializedEvent)
        {
            var @event = (IEvent)JsonConvert.DeserializeObject(serializedEvent.Data, GetEventTypeFrom(serializedEvent.EventType));
            AssignEventIdFromEventToPublishId(@event, serializedEvent);
            return @event;
        }

        private Type GetEventTypeFrom(string eventType)
        {
            return _assemblyContainingEvents.GetTypes()
                .Single(t => typeof(IEvent).IsAssignableFrom(t) && t.FullName == eventType);
        }

        private void AssignEventIdFromEventToPublishId(IEvent @event, Event eventToPublish)
        {
            @event.Id = eventToPublish.Id;
        }
    }
}