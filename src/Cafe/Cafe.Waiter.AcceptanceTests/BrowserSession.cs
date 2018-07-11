using System;
using OpenQA.Selenium.Chrome;

namespace Cafe.Waiter.AcceptanceTests
{
    public class BrowserSession : IDisposable
    {
        private readonly ChromeDriver _chromeDriver;

        public BrowserSession(ChromeDriver chromeDriver)
        {
            _chromeDriver = chromeDriver;
        }

        public OpenTabs OpenTabs => new OpenTabs(_chromeDriver);

        public void Dispose()
        {
            _chromeDriver?.Dispose();
        }
    }
}