using System;
using System.Linq;
using System.Reflection;
using CQ.Core;
using CQ.DAL.Serialized;
using Newtonsoft.Json;

namespace CQ.DAL
{
    public class EventToPublishSerializer : IEventToPublishSerializer
    {
        private readonly Assembly _assemblyToInspectForEvents;

        public EventToPublishSerializer(Assembly assemblyToInspectForEvents)
        {
            _assemblyToInspectForEvents = assemblyToInspectForEvents;
        }

        public IEvent Deserialize(EventToPublish eventToPublish)
        {
            var data = eventToPublish.Data;
            var eventTypeString = eventToPublish.EventType;
            var @event = (IEvent)JsonConvert.DeserializeObject(data, GetEventTypeFromString(eventTypeString));
            AssignEventIdFromEventToPublishId(@event, eventToPublish);
            return @event;
        }

        public EventToPublish Serialize(IEvent @event)
        {
            return new EventToPublish
            {
                Id = @event.Id,
                EventType = @event.GetType().FullName,
                Data = JsonConvert.SerializeObject(@event),
                Created = DateTime.Now
            };
        }

        private Type GetEventTypeFromString(string eventType)
        {
            return _assemblyToInspectForEvents.GetTypes()
                .Single(t => typeof(IEvent).IsAssignableFrom(t) && t.FullName == eventType);
        }

        private void AssignEventIdFromEventToPublishId(IEvent @event, EventToPublish eventToPublish)
        {
            @event.Id = eventToPublish.Id;
        }
    }
}