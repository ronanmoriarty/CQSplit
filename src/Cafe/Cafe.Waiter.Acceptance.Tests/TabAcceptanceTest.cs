using System.Threading;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Cafe.Waiter.Acceptance.Tests
{
    [TestFixture, Category(TestConstants.Acceptance)]
    public class TabAcceptanceTest
    {
        private IConfigurationRoot _configurationRoot;
        private const int TableNumber = 345;
        private const string Waiter = "TabAcceptanceTest";

        [SetUp]
        public void SetUp()
        {
            OpenTabs.DeleteTabsFor(Waiter);
            _configurationRoot = GetConfigurationRoot();
        }

        [Test]
        public void Created_tab_is_displayed_on_open_tabs_view()
        {
            using (var browserSession = new CafeWaiterWebsite(_configurationRoot)
                .CreateTab
                .WithTableNumber(TableNumber)
                .WithWaiter(Waiter)
                .AndSubmit())
            {
                AllowTimeForMessagesToBeConsumed();
                browserSession.RefreshPage();
                Assert.That(browserSession.OpenTabs.ContainsSingleTab.WithWaiter(Waiter).WithTableNumber(TableNumber));
            }
        }

        private void AllowTimeForMessagesToBeConsumed()
        {
            Thread.Sleep(5000);
        }

        private IConfigurationRoot GetConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appSettings.json")
                .Build();
        }
    }
}
