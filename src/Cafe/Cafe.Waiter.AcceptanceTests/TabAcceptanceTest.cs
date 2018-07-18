using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace Cafe.Waiter.AcceptanceTests
{
    [TestFixture, Category("AcceptanceTestInProgress")]
    public class TabAcceptanceTest
    {
        private IEnumerable<ExternalProcess> _externalProcesses;
        private const int TableNumber = 345;
        private const string Waiter = "TabAcceptanceTest";

        [SetUp]
        public void SetUp()
        {
            OpenTabs.DeleteTabsFor(Waiter);
            _externalProcesses = Start.AllWaiterServices();
        }

        [Test]
        public void Created_tab_is_displayed_on_open_tabs_view()
        {
            using (var browserSession = CafeWaiterWebsite
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

        [TearDown]
        public void TearDown()
        {
            foreach (var externalProcess in _externalProcesses)
            {
                externalProcess.Dispose();
            }
        }
    }
}
