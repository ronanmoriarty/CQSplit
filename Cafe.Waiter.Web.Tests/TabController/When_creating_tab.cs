using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cafe.Waiter.Commands;
using Cafe.Waiter.Contracts.Commands;
using Cafe.Waiter.Web.Models;
using CQRSTutorial.Messaging;
using MassTransit;
using NSubstitute;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests.TabController
{
    [TestFixture]
    public class When_creating_tab
    {
        private const string Waiter = "John";
        private const int TableNumber = 5;
        private Web.Api.TabController _tabController;
        private ISendEndpoint _endPoint;
        private ICommandSender _commandSender;
        private IEndpointProvider _endpointProvider;
        private CreateTabModel _model;

        [SetUp]
        public void SetUp()
        {
            _model = new CreateTabModel
            {
                Waiter = Waiter,
                TableNumber = TableNumber
            };

            _endPoint = Substitute.For<ISendEndpoint>();
            _endpointProvider = Substitute.For<IEndpointProvider>();
            _endpointProvider.GetSendEndpointFor(typeof(OpenTabCommand)).Returns(Task.FromResult(_endPoint));
            _commandSender = new CommandSender(_endpointProvider);
        }

        [Test]
        public async Task OpenTab_command_sent_to_message_bus_with_ids_set()
        {
            await WhenTabCreated();
            await _endPoint.Received().Send(Arg.Is<IOpenTabCommand>(command => PropertiesMatch(command))); // don't care too much about other values (TableNumber and waiter name) at the moment - happy setting them to arbitrary values for display purposes - no need to assert them.
        }

        private async Task WhenTabCreated()
        {
            _tabController = CreateTabController();
            await _tabController.Create(_model);
        }

        private Web.Api.TabController CreateTabController()
        {
            return new Web.Api.TabController(null, null,_commandSender, null);
        }

        private bool PropertiesMatch(IOpenTabCommand command)
        {
            return command.Id != Guid.Empty
                && command.AggregateId != Guid.Empty
                && command.Waiter == Waiter
                && command.TableNumber == TableNumber;
        }

        private Guid GetTabId()
        {
            var receivedCalls = _endPoint.ReceivedCalls();
            var sendCall = receivedCalls.Single(call => call.GetMethodInfo().Name == "Send");
            var arguments = sendCall.GetArguments();
            Assert.That(arguments[0] is IOpenTabCommand);
            Assert.That(arguments[1] is CancellationToken);
            var openTabCommand = (OpenTabCommand)arguments.First();
            return openTabCommand.AggregateId;
        }
    }
}
