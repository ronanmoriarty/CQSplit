namespace CQRSTutorial.DAL
{
    public class EntityFrameworkUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public EntityFrameworkUnitOfWorkFactory(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public IUnitOfWork Create()
        {
            return new EntityFrameworkUnitOfWork(_connectionStringProvider);
        }
    }
}