namespace Cafe.Waiter.AcceptanceTests
{
    public class ContainsSingleTabConstraintBuilder
    {
        private string _waiter;
        private int _tableNumber;

        public ContainsSingleTabConstraintBuilder WithWaiter(string waiter)
        {
            _waiter = waiter;
            return this;
        }

        public ContainsSingleTabConstraintBuilder WithTableNumber(int tableNumber)
        {
            _tableNumber = tableNumber;
            return this;
        }

        public static implicit operator bool(ContainsSingleTabConstraintBuilder builder)
        {
            return false;
        }
    }
}