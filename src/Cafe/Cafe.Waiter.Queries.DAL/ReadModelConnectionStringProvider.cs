using Cafe.DAL.Common;

namespace Cafe.Waiter.Queries.DAL
{
    public class ReadModelConnectionStringProvider
    {
        public static IConnectionStringProvider Instance = new ConnectionStringProviderFactory(ConfigurationRoot.Instance).GetConnectionStringProvider();
    }
}