using System;
using Cafe.Domain.Commands;
using Cafe.Waiter.Web.Controllers;
using CQRSTutorial.Core;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests
{
    [TestFixture]
    public class TabControllerTests
    {
        private TabController _tabController;
        private ICommandDispatcher _commandDispatcher;

        [SetUp]
        public void SetUp()
        {
            _commandDispatcher = Substitute.For<ICommandDispatcher>();
            _tabController = new TabController(_commandDispatcher);
        }

        [Test]
        public void Creating_new_tab_dispatches_OpenTab_command_with_ids_set()
        {
            _tabController.Create();
            _commandDispatcher.Received().Dispatch(Arg.Is<OpenTab>(command => HasIdPropertiesSet(command))); // don't care too much about other values (TableNumber and waiter name) at the moment - happy setting them to arbitrary values for display purposes - no need to assert them.
        }

        private bool HasIdPropertiesSet(OpenTab command)
        {
            return command.Id != Guid.Empty
                && command.AggregateId != Guid.Empty;
        }
    }
}
