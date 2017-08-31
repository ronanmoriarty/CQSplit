using System;
using System.Collections.Generic;
using CQRSTutorial.Core;
using log4net;

namespace CQRSTutorial.DAL
{
    public class EventHandler : IEventHandler
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IEventStore _eventStore;
        private readonly ILog _logger = LogManager.GetLogger(typeof(EventHandler));

        public EventHandler(IUnitOfWorkFactory unitOfWorkFactory, IEventStore eventStore)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
            _eventStore = eventStore;
        }

        public void Handle(IEnumerable<IEvent> events)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                unitOfWork.Start();
                unitOfWork.Enlist(_eventStore);
                try
                {
                    foreach (var @event in events)
                    {
                        _eventStore.Add(@event);
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
