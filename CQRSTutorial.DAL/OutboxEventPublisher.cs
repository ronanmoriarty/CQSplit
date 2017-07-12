using System;
using System.Collections.Generic;
using CQRSTutorial.Core;

namespace CQRSTutorial.DAL
{
    public class OutboxEventPublisher : IEventPublisher
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IEventRepository _eventRepository;
        private readonly IEventRepository _eventStore;

        public OutboxEventPublisher(IUnitOfWorkFactory unitOfWorkFactory, IEventRepository eventStore, IEventRepository eventRepository)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _eventRepository = eventRepository;
            _eventStore = eventStore;
        }

        public void Publish(IEnumerable<IEvent> events)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                unitOfWork.Start();
                _eventRepository.UnitOfWork = unitOfWork;
                _eventStore.UnitOfWork = unitOfWork;
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
