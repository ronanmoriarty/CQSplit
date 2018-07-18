using CQRSTutorial.DAL.Common;
using CQRSTutorial.DAL.Tests.Common;
using OpenQA.Selenium.Chrome;

namespace Cafe.Waiter.AcceptanceTests
{
    public class OpenTabs
    {
        private readonly ChromeDriver _chromeDriver;

        public OpenTabs(ChromeDriver chromeDriver)
        {
            _chromeDriver = chromeDriver;
        }

        public static void DeleteTabsFor(string waiter)
        {
            var connectionStringProvider = EnvironmentVariableConnectionStringProviderFactory.Get("CQRSTUTORIAL_CAFE_WAITER_READMODEL_CONNECTIONSTRING_OVERRIDE");
            var sqlExecutor = new SqlExecutor(new ConnectionStringOverride(connectionStringProvider.GetConnectionString()));
            var commandText = $"DELETE FROM dbo.OpenTabs WHERE [Data] LIKE '%\"Waiter\":\"{waiter}\"%'";
            sqlExecutor.ExecuteNonQuery(commandText);
        }

        public ContainsSingleTabConstraintBuilder ContainsSingleTab => new ContainsSingleTabConstraintBuilder(_chromeDriver);
    }
}