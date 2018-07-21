using CQRSTutorial.DAL.Common;
using Microsoft.Extensions.Configuration;

namespace Cafe.Waiter.Command.Service
{
    public class WriteModelConnectionStringProvider
    {
        private readonly IConfigurationRoot _configurationRoot;
        public static IConnectionStringProvider Instance = EnvironmentVariableConnectionStringProviderFactory.Get("CQRSTUTORIAL_CAFE_WAITER_WRITEMODEL_CONNECTIONSTRING_OVERRIDE");

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