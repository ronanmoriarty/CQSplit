using CQRSTutorial.DAL;
using CQRSTutorial.DAL.Common;

namespace CQRSTutorial.Publish.Tests
{
    public class WriteModelConnectionStringProvider
    {
        public static IConnectionStringProvider Instance = EnvironmentVariableConnectionStringProviderFactory.Get("CQRSTUTORIAL_CAFE_WAITER_WRITEMODEL_CONNECTIONSTRING_OVERRIDE");
    }
}