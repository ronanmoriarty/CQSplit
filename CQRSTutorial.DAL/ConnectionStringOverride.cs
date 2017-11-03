using CQRSTutorial.DAL.Common;

namespace CQRSTutorial.DAL
{
    public class ConnectionStringOverride : IConnectionStringProvider
    {
        private readonly string _connectionString;

        public ConnectionStringOverride(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}