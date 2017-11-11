using System;
using CQRSTutorial.DAL.Common;
using log4net;

namespace CQRSTutorial.DAL
{
    public class EventStoreUnitOfWork : IUnitOfWork
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(EventStoreUnitOfWork));

        public EventStoreUnitOfWork(IConnectionStringProvider connectionStringProvider)
        {
            EventStoreContext = new EventStoreContext(connectionStringProvider.GetConnectionString());
        }

        public EventStoreContext EventStoreContext { get; set; }

        public void Dispose()
        {
            EventStoreContext.Dispose();
        }

        public void Start()
        {
        }

        public void Commit()
        {
            EventStoreContext.SaveChanges();
        }

        public void Rollback()
        {
            Dispose();
        }

        public void Enroll(params IHaveUnitOfWork[] haveUnitOfWorks)
        {
            foreach (var haveUnitOfWork in haveUnitOfWorks)
            {
                haveUnitOfWork.UnitOfWork = this;
            }
        }

        public IUnitOfWork Enrolling(params IHaveUnitOfWork[] haveUnitOfWorks)
        {
            Enroll(haveUnitOfWorks);
            return this;
        }

        public void Execute(Action action)
        {
            Start();
            try
            {
                action();
                Commit();
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                Rollback();
            }
        }
    }
}