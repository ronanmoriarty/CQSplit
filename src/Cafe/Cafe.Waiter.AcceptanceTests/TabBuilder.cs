using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Cafe.Waiter.AcceptanceTests
{
    public class TabBuilder
    {
        private int _tableNumber;
        private string _waiter;
        private readonly ChromeDriver _chromeDriver;

        public TabBuilder()
        {
            _chromeDriver = new ChromeDriver();
        }

        public TabBuilder WithTableNumber(int tableNumber)
        {
            _tableNumber = tableNumber;
            return this;
        }

        public TabBuilder WithWaiter(string waiter)
        {
            _waiter = waiter;
            return this;
        }

        public BrowserSession AndSubmit()
        {
            try
            {
                NavigateToOpenTabs();
                SetWaiter();
                SetTableNumber();
                Submit();
                return new BrowserSession(_chromeDriver);
            }
            catch (NoSuchElementException)
            {
                _chromeDriver?.Dispose();
                throw;
            }
        }

        private void NavigateToOpenTabs()
        {
            _chromeDriver.Url = "http://localhost:5000/app/index.html#!/tabs";
        }

        private void SetTableNumber()
        {
            var tableNumberTextBox = _chromeDriver.FindElementById("tableNumber");
            tableNumberTextBox.SendKeys(_tableNumber.ToString());
        }

        private void SetWaiter()
        {
            //TODO allow waiter to be set on UI.
        }

        private void Submit()
        {
            var createTabButton = _chromeDriver.FindElement(By.XPath("//input[@type=\"submit\"]"));
            createTabButton.Click();
        }
    }
}