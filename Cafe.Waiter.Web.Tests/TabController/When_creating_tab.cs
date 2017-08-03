using System;
using System.Linq;
using System.Web.Mvc;
using Cafe.Domain.Commands;
using CQRSTutorial.Core;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests.TabController
{
    [TestFixture]
    public class When_creating_tab
    {
        private Controllers.TabController _tabController;
        private IMessageBus _messageBus;
        private ActionResult _actionResult;

        [SetUp]
        public void SetUp()
        {
            _messageBus = Substitute.For<IMessageBus>();
            _tabController = new Controllers.TabController(_messageBus);
            _actionResult = _tabController.Create();
        }

        [Test]
        public void OpenTab_command_sent_to_message_bus_with_ids_set()
        {
            _messageBus.Received().Send(Arg.Is<OpenTab>(command => HasIdPropertiesSet(command))); // don't care too much about other values (TableNumber and waiter name) at the moment - happy setting them to arbitrary values for display purposes - no need to assert them.
        }

        [Test]
        public void Redirects_to_index_action_indicating_id_of_newly_created_tab()
        {
            var tabId = GetTabId();
            var redirectToRouteResult = (RedirectToRouteResult)_actionResult;
            Assert.That(redirectToRouteResult.RouteValues["action"], Is.EqualTo("Index"));
            Assert.That(redirectToRouteResult.RouteValues["tabId"], Is.EqualTo(tabId));
        }

        private bool HasIdPropertiesSet(OpenTab command)
        {
            return command.Id != Guid.Empty
                && command.AggregateId != Guid.Empty;
        }

        private Guid GetTabId()
        {
            var openTabCommand = (OpenTab)_messageBus.ReceivedCalls().Single().GetArguments().Single();
            return openTabCommand.AggregateId;
        }
    }
}
