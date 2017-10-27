using System.Data.SqlClient;

namespace CQRSTutorial.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SqlConnection _connection;
        private SqlTransaction _transaction;

        public UnitOfWork(SqlConnection connection)
        {
            _connection = connection;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }

        public void Start()
        {
            _transaction = _connection?.BeginTransaction();
        }

        public void Commit()
        {
            _transaction?.Commit();
        }

        public void Rollback()
        {
            _transaction?.Rollback();
        }

        public void Enlist(IHaveUnitOfWork haveUnitOfWork)
        {
            haveUnitOfWork.UnitOfWork = this;
        }
    }
}