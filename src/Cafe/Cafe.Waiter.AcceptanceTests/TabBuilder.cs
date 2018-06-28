using System;

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
        }
    }
}