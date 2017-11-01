using Cafe.Waiter.Commands;
using Cafe.Waiter.Events;
using NUnit.Framework;

namespace Cafe.Waiter.Domain.Tests
{
    [TestFixture]
    public class OpenTabTests : EventTestsBase<Tab, OpenTabCommand>
    {
        private readonly int _tableNumber = 123;
        private readonly string _waiter = "John Smith";

        [Test]
        public void CanOpenANewTab()
        {
            When(new OpenTabCommand
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

        protected override bool CanUsePreregisteredCommandHandlersToHandleCommand()
        {
            return true;
        }
    }
}