using System;
using System.Collections.Generic;
using CQRSTutorial.Core;

namespace CQRSTutorial.DAL
{
    public class OutboxEventPublisher : IEventPublisher
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IEventRepository _eventRepository;
        private readonly IEventStore _eventStore;

        public OutboxEventPublisher(IUnitOfWorkFactory unitOfWorkFactory, IEventStore eventStore, IEventRepository eventRepository)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _eventStore = eventStore;
            _eventRepository = eventRepository;
        }

        public void Publish(IEnumerable<IEvent> events)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                unitOfWork.Start();
                unitOfWork.Enlist(_eventRepository);
                unitOfWork.Enlist(_eventStore);
                try
                {
                    foreach (var @event in events)
                    {
                        _eventStore.Add(@event);
                        _eventRepository.Add(@event);
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
