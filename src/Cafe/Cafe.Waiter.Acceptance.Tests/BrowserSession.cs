using System;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Cafe.Waiter.Acceptance.Tests
{
    public class BrowserSession : IDisposable
    {
        private readonly RemoteWebDriver _webDriver;

        public BrowserSession(RemoteWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public OpenTabs OpenTabs => new OpenTabs(_webDriver);

        public void Dispose()
        {
            _webDriver?.Dispose();
        }

        public void RefreshPage()
        {
            _webDriver.Navigate().Refresh();
        }
    }
}