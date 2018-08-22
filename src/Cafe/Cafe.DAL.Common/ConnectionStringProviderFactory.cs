using Microsoft.Extensions.Configuration;

namespace Cafe.DAL.Common
{
    public class ConnectionStringProviderFactory
    {
        private readonly IConfigurationRoot _configurationRoot;

        public ConnectionStringProviderFactory(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        public string GetConnectionString()
        {
            return _configurationRoot["connectionString"];
        }
    }
}
