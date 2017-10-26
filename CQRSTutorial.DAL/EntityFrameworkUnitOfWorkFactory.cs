namespace CQRSTutorial.DAL
{
    public class EntityFrameworkUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IConnectionStringProviderFactory _connectionStringProviderFactory;

        public EntityFrameworkUnitOfWorkFactory(IConnectionStringProviderFactory connectionStringProviderFactory)
        {
            _connectionStringProviderFactory = connectionStringProviderFactory;
        }

        public IUnitOfWork Create()
        {
            return new EntityFrameworkUnitOfWork(_connectionStringProviderFactory);
        }
    }
}