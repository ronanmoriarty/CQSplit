using System.Configuration;

namespace CQRSTutorial.DAL
{
    public class AppConfigConnectionStringProvider : IConnectionStringProvider
    {
        private readonly string _connectionStringKey;

        public AppConfigConnectionStringProvider(string connectionStringKey)
        {
            _connectionStringKey = connectionStringKey;
        }

        public string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[_connectionStringKey].ConnectionString;
        }
    }
}