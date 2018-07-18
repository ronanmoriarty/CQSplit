using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace CQRSTutorial.Core.Tests
{
    [TestFixture]
    public class CommandRouterTests
    {
        private CommandRouter _commandRouter;
        private ICommandHandlerFactory _commandHandlerFactory;

        [SetUp]
        public void SetUp()
        {
            _commandHandlerFactory = Substitute.For<ICommandHandlerFactory>();
            var handler1 = new Handler1();
            var handler2 = new Handler2();
            var commandHandlerProvider = new CommandHandlerProvider(_commandHandlerFactory);
            commandHandlerProvider.RegisterCommandHandler(handler1);
            commandHandlerProvider.RegisterCommandHandler(handler2);
            _commandRouter = new CommandRouter(Substitute.For<IEventHandler>(), commandHandlerProvider);
        }

        [Test]
        public void ExceptionThrownIfMoreThanOneHandlerCanHandleCommand()
        {
            Assert.That(() => _commandRouter.Route(
                new TestCommand
                {
                    Id = Guid.NewGuid()
                }),
                Throws.Exception.InstanceOf<ArgumentException>().With.Message.EqualTo($"More than one type found that can handle {typeof(TestCommand).FullName} commands"));
        }

        [Test]
        public void ExceptionThrownIfNothingCanHandleCommand()
        {
            _commandHandlerFactory.CreateHandlerFor(Arg.Any<UnhandledTestCommand>()).Returns((ICommandHandler<UnhandledTestCommand>)null);
            Assert.That(() => _commandRouter.Route(
                new UnhandledTestCommand
                {
                    Id = Guid.NewGuid()
                }),
                Throws.Exception.InstanceOf<ArgumentException>().With.Message.EqualTo($"Could not find any handler to handle command of type {typeof(UnhandledTestCommand)}"));
        }

        [Test]
        public void ExceptionThrownIfCommandDoesNotHaveIdSet()
        {
            Assert.That(() => _commandRouter.Route(
                new TestCommand()),
                Throws.Exception.InstanceOf<ArgumentException>().With.Message.EqualTo("Command does not have Id set."));
        }

        internal class Handler1 : ICommandHandler<TestCommand>
        {
            public IEnumerable<IEvent> Handle(TestCommand command)
            {
                throw new NotImplementedException();
            }

            public bool CanHandle(ICommand command)
            {
                return command.GetType() == typeof(TestCommand);
            }
        }

        internal class Handler2 : ICommandHandler<TestCommand>
        {
            public IEnumerable<IEvent> Handle(TestCommand command)
            {
                throw new NotImplementedException();
            }

            public bool CanHandle(ICommand command)
            {
                return command.GetType() == typeof(TestCommand);
            }
        }

        internal class TestCommand : ICommand
        {
            public Guid Id { get; set; }
            public Guid AggregateId { get; set; }
        }

        public class UnhandledTestCommand : ICommand
        {
            public Guid Id { get; set; }
            public Guid AggregateId { get; set; }
        }
    }
}