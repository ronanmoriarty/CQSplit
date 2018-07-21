using CQRSTutorial.DAL.Common;
using Microsoft.Extensions.Configuration;

namespace Cafe.Waiter.Command.Service
{
    public class WriteModelConnectionStringProvider
    {
        private readonly IConfigurationRoot _configurationRoot;

        public WriteModelConnectionStringProvider(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        public IConnectionStringProvider GetConnectionStringProvider()
        {
            return new ConnectionStringOverride(_configurationRoot["connectionString"]);
        }
    }
}