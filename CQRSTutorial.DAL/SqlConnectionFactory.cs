using System.Data.SqlClient;

namespace CQRSTutorial.DAL
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly IConnectionStringProviderFactory _connectionStringProviderFactory;

        public SqlConnectionFactory(IConnectionStringProviderFactory connectionStringProviderFactory)
        {
            _connectionStringProviderFactory = connectionStringProviderFactory;
        }

        public SqlConnection Create()
        {
            return new SqlConnection(_connectionStringProviderFactory.GetConnectionStringProvider().GetConnectionString());
        }
    }
}