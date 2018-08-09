using System;
using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;

namespace CQ.Core.Tests
{
    [TestFixture]
    public class CommandRouterTests
    {
        private CommandRouter _commandRouter;
        private ICommandHandlerFactory _commandHandlerFactory;
        private ICommandHandler<Test2Command> _test2CommandHandler;
        private Test2Command _test2Command;
        private IEventHandler _eventHandler;
        private Test2Event[] _events;

        [SetUp]
        public void SetUp()
        {
            _commandHandlerFactory = Substitute.For<ICommandHandlerFactory>();
            var testCommandHandler1 = Substitute.For<ICommandHandler<TestCommand>>();
            testCommandHandler1.CanHandle(Arg.Any<TestCommand>()).Returns(true);
            var testCommandHandler2 = Substitute.For<ICommandHandler<TestCommand>>();
            testCommandHandler2.CanHandle(Arg.Any<TestCommand>()).Returns(true);
            _test2CommandHandler = Substitute.For<ICommandHandler<Test2Command>>();
            _test2CommandHandler.CanHandle(Arg.Any<Test2Command>()).Returns(true);
            _test2Command = new Test2Command
            {
                Id = Guid.NewGuid()
            };
            _events = new[] { new Test2Event() };
            _test2CommandHandler.Handle(_test2Command).Returns(_events);
            var commandHandlerProvider = new CommandHandlerProvider(_commandHandlerFactory);
            commandHandlerProvider.RegisterCommandHandler(testCommandHandler1);
            commandHandlerProvider.RegisterCommandHandler(testCommandHandler2);
            commandHandlerProvider.RegisterCommandHandler(_test2CommandHandler);
            _eventHandler = Substitute.For<IEventHandler>();
            _commandRouter = new CommandRouter(_eventHandler, commandHandlerProvider);
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

        [Test]
        public void CommandHandledWhenSingleMatchingCommandHandlerFound()
        {
            _commandRouter.Route(_test2Command);

            _test2CommandHandler.Received(1).Handle(_test2Command);
        }

        [Test]
        public void EventsArePassedToEventHandler()
        {
            _commandRouter.Route(_test2Command);

            _test2CommandHandler.Received(1).Handle(_test2Command);

            _eventHandler.Received(1).Handle(_events);
        }

        [ExcludeFromCodeCoverage]
        public class TestCommand : ICommand
        {
            public Guid Id { get; set; }
            public Guid AggregateId { get; set; }
        }

        [ExcludeFromCodeCoverage]
        public class Test2Command : ICommand
        {
            public Guid Id { get; set; }
            public Guid AggregateId { get; set; }
        }

        [ExcludeFromCodeCoverage]
        public class Test2Event : IEvent
        {
            public Guid Id { get; set; }
            public Guid AggregateId { get; set; }
            public Guid CommandId { get; set; }
        }

        [ExcludeFromCodeCoverage]
        public class UnhandledTestCommand : ICommand
        {
            public Guid Id { get; set; }
            public Guid AggregateId { get; set; }
        }
    }
}