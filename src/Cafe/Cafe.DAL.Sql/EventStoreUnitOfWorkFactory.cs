using CQRSTutorial.DAL;
using Cafe.DAL.Common;

namespace Cafe.DAL.Sql
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