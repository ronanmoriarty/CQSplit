using System;
using CQRSTutorial.Core;
using Newtonsoft.Json;

namespace CQRSTutorial.DAL
{
    public class EventRepository : IEventRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(IEvent @event)
        {
            using (var session = _unitOfWork.GetSession())
            {
                var eventDescriptor = new EventDescriptor
                {
                    EventType = @event.GetType(),
                    Data = JsonConvert.SerializeObject(@event)
                };
                session.SaveOrUpdate(eventDescriptor);
                UpdateEventIdToReflectIdAssignedByNHibernateToEventDescriptor(@event, eventDescriptor);
                session.Flush();
            }
        }

        public IEvent Read(Guid id)
        {
            using (var session = _unitOfWork.GetSession())
            {
                var eventDescriptor = session.Get<EventDescriptor>(id);
                var @event = (IEvent)JsonConvert.DeserializeObject(eventDescriptor.Data, eventDescriptor.EventType);
                AssignEventIdFromEventDescriptorId(@event, eventDescriptor);
                return @event;
            }
        }

        private void UpdateEventIdToReflectIdAssignedByNHibernateToEventDescriptor(IEvent @event,
            EventDescriptor eventDescriptor)
        {
            // We're not saving the event itself - so the event's id doesn't get updated automatically by NHibernate. Only the EventDescriptor's Id gets updated during saving.
            @event.Id = eventDescriptor.Id;
        }

        private void AssignEventIdFromEventDescriptorId(IEvent @event, EventDescriptor eventDescriptor)
        {
            @event.Id = eventDescriptor.Id;
        }
    }
}
