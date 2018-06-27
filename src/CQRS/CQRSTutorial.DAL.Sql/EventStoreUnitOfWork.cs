using System;
using CQRSTutorial.DAL.Common;
using log4net;

namespace CQRSTutorial.DAL.Sql
{
    public class EventStoreUnitOfWork : IUnitOfWork
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(EventStoreUnitOfWork));

        public EventStoreUnitOfWork(IConnectionStringProvider connectionStringProvider)
        {
            EventStoreDbContext = new EventStoreDbContext(connectionStringProvider.GetConnectionString());
        }

        public EventStoreDbContext EventStoreDbContext { get; set; }

        public void Dispose()
        {
            EventStoreDbContext.Dispose();
        }

        public void Commit()
        {
            EventStoreDbContext.SaveChanges();
        }

        private void Rollback()
        {
            Dispose();
        }

        public IUnitOfWork Enrolling(params IHaveUnitOfWork[] haveUnitOfWorks)
        {
            Enroll(haveUnitOfWorks);
            return this;
        }

        private void Enroll(params IHaveUnitOfWork[] haveUnitOfWorks)
        {
            foreach (var haveUnitOfWork in haveUnitOfWorks)
            {
                haveUnitOfWork.UnitOfWork = this;
            }
        }

        public void ExecuteInTransaction(Action action)
        {
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