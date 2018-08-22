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
            var connectionString = new ConnectionStringProviderFactory(ConfigurationRoot.Instance).GetConnectionString();
            var sqlExecutor = new SqlExecutor(connectionString);
            var commandText = $"DELETE FROM dbo.OpenTabs WHERE [Data] LIKE '%\"Waiter\":\"{waiter}\"%'";
            sqlExecutor.ExecuteNonQuery(commandText);
        }

        public ContainsSingleTabConstraintBuilder ContainsSingleTab => new ContainsSingleTabConstraintBuilder(_chromeDriver);
    }
}