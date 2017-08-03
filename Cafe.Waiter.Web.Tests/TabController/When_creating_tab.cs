﻿using System;
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
        private ICommandDispatcher _commandDispatcher;
        private ActionResult _actionResult;

        [SetUp]
        public void SetUp()
        {
            _commandDispatcher = Substitute.For<ICommandDispatcher>();
            _tabController = new Controllers.TabController(_commandDispatcher);
            _actionResult = _tabController.Create();
        }

        [Test]
        public void OpenTab_command_dispatched_with_ids_set()
        {
            _commandDispatcher.Received().Dispatch(Arg.Is<OpenTab>(command => HasIdPropertiesSet(command))); // don't care too much about other values (TableNumber and waiter name) at the moment - happy setting them to arbitrary values for display purposes - no need to assert them.
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
            var commands = (ICommand[])_commandDispatcher.ReceivedCalls().Single().GetArguments().Single();
            var openTabCommand = (OpenTab)commands[0];
            return openTabCommand.AggregateId;
        }
    }
}
