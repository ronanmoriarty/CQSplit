using CQRSTutorial.DAL;

namespace Cafe.Waiter.Command.Service
{
    public static class WriteModelConnectionStringProvider
    {
        public static IConnectionStringProvider Instance = EnvironmentVariableConnectionStringProviderFactory.Get("CQRSTUTORIAL_CAFE_WAITER_WRITEMODEL_CONNECTIONSTRING_OVERRIDE");
    }
}