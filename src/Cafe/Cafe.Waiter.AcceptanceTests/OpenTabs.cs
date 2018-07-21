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
            var connectionStringProvider = new ConnectionStringProviderFactory(ConfigurationRoot.Instance).GetConnectionStringProvider();
            var sqlExecutor = new SqlExecutor(connectionStringProvider);
            var commandText = $"DELETE FROM dbo.OpenTabs WHERE [Data] LIKE '%\"Waiter\":\"{waiter}\"%'";
            sqlExecutor.ExecuteNonQuery(commandText);
        }

        public ContainsSingleTabConstraintBuilder ContainsSingleTab => new ContainsSingleTabConstraintBuilder(_chromeDriver);
    }
}