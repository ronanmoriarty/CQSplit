using System;
using Cafe.Waiter.Commands;
using CQSplit.Core;
using NUnit.Framework;

namespace Cafe.Waiter.Command.Service.Tests
{
    [TestFixture]
    public class ContainerTests
    {
        private readonly Guid _commandId = new Guid("7CBBB931-EA8D-414E-9A7C-801FABFB7D5C");
        private readonly Guid _aggregateId = new Guid("F51210BC-4D80-4F6A-990F-D55E891DA251");

        [Test]
        public void Can_resolve_WaiterCommandService()
        {
            var waiterService = Container.Instance.Resolve<WaiterCommandService>();

            Assert.That(waiterService, Is.Not.Null);
        }

        [Test]
        public void OpenTab_command_can_be_handled_by_registered_CommandHandlerProvider()
        {
            var commandHandlerProvider = Container.Instance.Resolve<ICommandHandlerProvider>();
            var commandHandler = commandHandlerProvider.GetCommandHandler(new OpenTabCommand
            {
                Id = _commandId,
                AggregateId = _aggregateId
            });

            Assert.IsNotNull(commandHandler);
        }
    }
}
