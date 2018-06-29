using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Cafe.Waiter.AcceptanceTests
{
    public class TabBuilder
    {
        private int _tableNumber;
        private string _waiter;
        private TimeSpan _timeSpan;

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

        public void WaitingAMaximumOf(TimeSpan timeSpan)
        {
            _timeSpan = timeSpan;
            var chromeDriver = new ChromeDriver
            {
                Url = "http://localhost:5000/app/index.html#!/tabs"
            };
            var tableNumberTextBox = chromeDriver.FindElementById("tableNumber");
            tableNumberTextBox.SendKeys(_tableNumber.ToString());
            var createTabButton = chromeDriver.FindElement(By.XPath("//input[@type=\"submit\"]"));
            createTabButton.Click();
            chromeDriver.FindElementByXPath($"//td[text() = \"{_waiter}\"]");
            chromeDriver.Close();
        }
    }
}