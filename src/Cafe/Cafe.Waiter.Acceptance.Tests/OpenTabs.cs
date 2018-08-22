using Cafe.DAL.Common;
using Cafe.DAL.Tests.Common;
using OpenQA.Selenium.Chrome;

namespace Cafe.Waiter.Acceptance.Tests
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
            var sqlExecutor = new SqlExecutor(connectionStringProvider.GetConnectionString());
            var commandText = $"DELETE FROM dbo.OpenTabs WHERE [Data] LIKE '%\"Waiter\":\"{waiter}\"%'";
            sqlExecutor.ExecuteNonQuery(commandText);
        }

        public ContainsSingleTabConstraintBuilder ContainsSingleTab => new ContainsSingleTabConstraintBuilder(_chromeDriver);
    }
}