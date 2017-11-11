using System.Collections.Generic;
using CQRSTutorial.DAL.Common;

namespace CQRSTutorial.DAL
{
    public class EntityFrameworkUnitOfWork : IUnitOfWork
    {
        public EntityFrameworkUnitOfWork(IConnectionStringProvider connectionStringProvider)
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
    }
}