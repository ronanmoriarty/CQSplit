using Cafe.Domain.Commands;
using Cafe.Domain.Events;
using NUnit.Framework;

namespace Cafe.Domain.Tests
{
    [TestFixture]
    public class OpenTabTests : EventTestsBase<Tab, OpenTab>
    {
        private readonly int _tableNumber = 123;
        private readonly string _waiter = "John Smith";

        [Test]
        public void CanOpenANewTab()
        {
            When(new OpenTab
            {
                Id = CommandId,
                AggregateId = AggregateId,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });

            Then(new TabOpened
            {
                AggregateId = AggregateId,
                CommandId = CommandId,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });
        }
    }
}