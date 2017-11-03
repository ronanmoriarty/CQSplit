using CQRSTutorial.DAL;

namespace Cafe.Waiter.Queries.DAL
{
    public class ReadModelConnectionStringProvider
    {
        public static IConnectionStringProvider Instance = EnvironmentVariableConnectionStringProviderFactory.Get("CQRSTUTORIAL_CAFE_WAITER_READMODEL_CONNECTIONSTRING_OVERRIDE");
    }
}