using System;
using System.Linq;
using System.Reflection;
using CQRSTutorial.Core;
using Newtonsoft.Json;

namespace CQRSTutorial.DAL
{
    public class EventToPublishMapper
    {
        private readonly Assembly _assemblyToInspectForEvents;

        public EventToPublishMapper(Assembly assemblyToInspectForEvents)
        {
            _assemblyToInspectForEvents = assemblyToInspectForEvents;
        }

        public IEvent MapToEvent(EventToPublish eventToPublish)
        {
            var @event = (IEvent)JsonConvert.DeserializeObject(eventToPublish.Data, GetEventTypeFrom(eventToPublish.EventType));
            AssignEventIdFromEventToPublishId(@event, eventToPublish);
            return @event;
        }

        private Type GetEventTypeFrom(string eventType)
        {
            return _assemblyToInspectForEvents.GetTypes()
                .Single(t => typeof(IEvent).IsAssignableFrom(t) && t.Name == eventType);
        }

        private void AssignEventIdFromEventToPublishId(IEvent @event, EventToPublish eventToPublish)
        {
            @event.Id = eventToPublish.Id;
        }
    }
}