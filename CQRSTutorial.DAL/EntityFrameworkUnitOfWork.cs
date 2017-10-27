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

        public void Enlist(IHaveUnitOfWork haveUnitOfWork)
        {
            haveUnitOfWork.UnitOfWork = this;
        }
    }
}