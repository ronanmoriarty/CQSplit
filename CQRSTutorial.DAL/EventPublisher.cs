using System;
using System.Collections.Generic;
using CQRSTutorial.Core;
using log4net;

namespace CQRSTutorial.DAL
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IEventToPublishRepository _eventToPublishRepository;
        private readonly IEventStore _eventStore;
        private readonly ILog _logger = LogManager.GetLogger(typeof(EventPublisher));

        public EventPublisher(IUnitOfWorkFactory unitOfWorkFactory, IEventStore eventStore, IEventToPublishRepository eventToPublishRepository)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _eventStore = eventStore;
            _eventToPublishRepository = eventToPublishRepository;
        }

        public void Publish(IEnumerable<IEvent> events)
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
                    _logger.Error(exception);
                    unitOfWork.Rollback();
                }
            }
        }
    }
}
