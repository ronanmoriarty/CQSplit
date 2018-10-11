using OpenQA.Selenium.Remote;

namespace Cafe.Waiter.Acceptance.Tests
{
    public interface IWebDriverFactory
    {
        RemoteWebDriver CreateWebDriver();
    }
}