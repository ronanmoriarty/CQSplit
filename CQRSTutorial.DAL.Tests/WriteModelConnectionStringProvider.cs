namespace CQRSTutorial.DAL.Tests
{
    public class WriteModelConnectionStringProvider
    {
        private const string EnvironmentVariableKey = "CQRSTUTORIAL_CAFE_WAITER_WRITEMODEL_CONNECTIONSTRING_OVERRIDE";

        public static IConnectionStringProvider Instance = EnvironmentVariableConnectionStringProviderFactory.Get(EnvironmentVariableKey);
    }
}