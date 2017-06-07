using System;
using Cafe.Domain.Commands;
using Cafe.Domain.Events;
using NUnit.Framework;

namespace Cafe.Domain.Tests
{
    [TestFixture]
    public class TabTests : EventTestsBase<Tab, OpenTab>
    {
        private readonly Guid _id = new Guid("91EBA94D-3A5F-45FD-BEC4-712E631C732C");
        private readonly int _tableNumber = 123;
        private readonly string _waiter = "John Smith";

        [Test]
        public void CanOpenANewTab()
        {
            WhenCommandReceived(new OpenTab
            {
                Id = _id,
                TableNumber = _tableNumber,
                Waiter = _waiter
            });

            AssertEventPublished<TabOpened>(AssertTabOpened);
        }

        private bool AssertTabOpened(TabOpened tabOpened)
        {
            return tabOpened.Id == _id
                   && tabOpened.TableNumber == _tableNumber
                   && tabOpened.Waiter == _waiter;
        }
    }
}
