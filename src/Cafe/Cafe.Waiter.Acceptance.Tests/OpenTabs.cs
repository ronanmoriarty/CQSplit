using Cafe.DAL.Tests.Common;
using OpenQA.Selenium.Remote;

namespace Cafe.Waiter.Acceptance.Tests
{
    public class OpenTabs
    {
        private readonly RemoteWebDriver _webDriver;

        public OpenTabs(RemoteWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public static void DeleteTabsFor(string waiter)
        {
            var sqlExecutor = new SqlExecutor();
            var commandText = $"DELETE FROM dbo.OpenTabs WHERE [Data] LIKE '%\"Waiter\":\"{waiter}\"%'";
            sqlExecutor.ExecuteNonQuery(commandText);
        }

        public ContainsSingleTabConstraintBuilder ContainsSingleTab => new ContainsSingleTabConstraintBuilder(_webDriver);
    }
}