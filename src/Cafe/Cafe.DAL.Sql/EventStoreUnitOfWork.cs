using System;
using CQSplit.DAL;
using NLog;

namespace Cafe.DAL.Sql
{
    public class EventStoreUnitOfWork : IUnitOfWork
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public EventStoreUnitOfWork(string connectionString)
        {
            EventStoreDbContext = new EventStoreDbContext(connectionString);
            LogDbContextHashCodeForMultithreadingDiagnosticPurposes();
        }

        private void LogDbContextHashCodeForMultithreadingDiagnosticPurposes()
        {
            // We're occasionally getting the following error:

            //2018 - 07 - 11 15:12:19,384[6] ERROR CQSplit.DAL.Sql.EventStoreUnitOfWork[(null)] - System.InvalidOperationException: An attempt was made to use the context while it is being configured.A DbContext instance cannot be used inside OnConfiguring since it is still being configured at this point.This can happen if a second operation is started on this context before a previous operation completed. Any instance members are not guaranteed to be thread safe.
            // at Microsoft.EntityFrameworkCore.DbContext.get_InternalServiceProvider()
            // at Microsoft.EntityFrameworkCore.DbContext.get_DbContextDependencies()
            // at Microsoft.EntityFrameworkCore.DbContext.EntryWithoutDetectChanges[TEntity](TEntity entity)
            // at Microsoft.EntityFrameworkCore.DbContext.SetEntityState[TEntity](TEntity entity, EntityState entityState)
            // at CQSplit.DAL.Sql.EventRepository.Add(IEvent event)
            // at CQSplit.DAL.CompositeEventStore.Add(IEvent event)
            // at CQSplit.DAL.EventHandler.<>c__DisplayClass3_0.<Handle>b__0()
            // at CQSplit.DAL.Sql.EventStoreUnitOfWork.ExecuteInTransaction(Action action)

            // This suggests that a single instance of EventStoreDbContext is being reused across different threads (though I can't see how).
            // If we see this error again, the following log should help indicate if it's the same instance being shared, or different instances using some shared underlying dependency.

            _logger.Debug($"EventStoreDbContext.GetHashCode():{EventStoreDbContext.GetHashCode()}");
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

        private void Rollback()
        {
            Dispose();
        }
    }
}