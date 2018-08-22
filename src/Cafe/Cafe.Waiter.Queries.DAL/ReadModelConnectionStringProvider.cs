using Cafe.DAL.Common;

namespace Cafe.Waiter.Queries.DAL
{
    public class ReadModelConnectionStringProvider
    {
        public static string ConnectionString = ConfigurationRoot.Instance["connectionString"];
    }
}