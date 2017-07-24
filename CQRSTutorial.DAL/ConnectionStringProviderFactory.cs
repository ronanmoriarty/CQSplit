using System;

namespace CQRSTutorial.DAL
{
    public class ConnectionStringProviderFactory : IConnectionStringProviderFactory
    {
        private readonly string _overrideKey;
        private readonly string _connectionStringKey;

        public ConnectionStringProviderFactory(string connectionStringKey, string overrideKey)
        {
            _overrideKey = overrideKey;
            _connectionStringKey = connectionStringKey;
        }

        public IConnectionStringProvider GetConnectionStringProvider()
        {
            var connectionString = Environment.GetEnvironmentVariable(_overrideKey, EnvironmentVariableTarget.Machine);
            if (!string.IsNullOrEmpty(connectionString))
            {
                return new ConnectionStringOverride(connectionString);
            }

            return new AppConfigConnectionStringProvider(_connectionStringKey);
        }
    }
}