﻿using System;
using System.Linq;
using System.Reflection;
using CQRSTutorial.Core;
using Newtonsoft.Json;

namespace CQRSTutorial.DAL
{
    public class EventMapper
    {
        private readonly Assembly _assemblyContainingEvents;

        public EventMapper(Assembly assemblyContainingEvents)
        {
            _assemblyContainingEvents = assemblyContainingEvents;
        }

        public IEvent MapToEvent(Event storedEvent)
        {
            var @event = (IEvent)JsonConvert.DeserializeObject(storedEvent.Data, GetEventTypeFrom(storedEvent.EventType));
            AssignEventIdFromEventToPublishId(@event, storedEvent);
            return @event;
        }

        private Type GetEventTypeFrom(string eventType)
        {
            return _assemblyContainingEvents.GetTypes()
                .Single(t => typeof(IEvent).IsAssignableFrom(t) && t.Name == eventType);
        }

        private void AssignEventIdFromEventToPublishId(IEvent @event, Event eventToPublish)
        {
            @event.Id = eventToPublish.Id;
        }
    }
}