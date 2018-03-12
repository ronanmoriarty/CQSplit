using CQRSTutorial.DAL.Common;

namespace CQRSTutorial.DAL
{
    public class EventStoreUnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public EventStoreUnitOfWorkFactory(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public IUnitOfWork Create()
        {
            return new EventStoreUnitOfWork(_connectionStringProvider);
        }
    }
}