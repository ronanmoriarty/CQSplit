using System;
using System.Collections.Generic;
using CQRSTutorial.Core;

namespace CQRSTutorial.DAL
{
    public class EventReceiver : IEventReceiver
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IEventToPublishRepository _eventToPublishRepository;
        private readonly IEventStore _eventStore;

        public EventReceiver(IUnitOfWorkFactory unitOfWorkFactory, IEventStore eventStore, IEventToPublishRepository eventToPublishRepository)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _eventStore = eventStore;
            _eventToPublishRepository = eventToPublishRepository;
        }

        public void Receive(IEnumerable<IEvent> events)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                unitOfWork.Start();
                unitOfWork.Enlist(_eventToPublishRepository);
                unitOfWork.Enlist(_eventStore);
                try
                {
                    foreach (var @event in events)
                    {
                        _eventStore.Add(@event);
                        _eventToPublishRepository.Add(@event);
                    }
                    unitOfWork.Commit();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    unitOfWork.Rollback();
                }
            }
        }
    }
}
