using System;
using NUnit.Framework;

namespace Cafe.Waiter.AcceptanceTests
{
    [TestFixture, Category("AcceptanceTestInProgress")]
    public class TabAcceptanceTest
    {
        private const int TableNumber = 345;
        private const string Waiter = "TabAcceptanceTest";

        [SetUp]
        public void SetUp()
        {
            OpenTabs.DeleteTabsFor(Waiter);
            Start.AllWaiterServices();
        }

        [Test]
        public void Created_tab_is_displayed_on_open_tabs_view()
        {
            CafeWaiterWebsite.CreateTab
                .WithTableNumber(TableNumber)
                .WithWaiter(Waiter)
                .WaitingAMaximumOf(TimeSpan.FromSeconds(60));

            Assert.That(CafeWaiterWebsite.OpenTabs.ContainsSingleTab.WithWaiter(Waiter).WithTableNumber(TableNumber));
        }
    }
}
