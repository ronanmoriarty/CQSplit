using System;
using Cafe.Domain.Commands;
using Cafe.Domain.Exceptions;
using NUnit.Framework;

namespace Cafe.Domain.Tests
{
    [TestFixture]
    public class PlaceOrderTests : EventTestsBase<Tab>
    {
        private readonly Guid _id = new Guid("44745A7A-6493-46C4-AA73-13AB1A2A5CDA");

        [Test]
        public void CannotOrderWhenTabHasNotBeenOpenedYet()
        {
            Assert.That(() => WhenCommandReceived(new PlaceOrder {Id = _id, Items = null}), Throws.Exception.InstanceOf<TabNotOpen>());
        }
    }
}
