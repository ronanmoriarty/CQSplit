using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CQRSTutorial.Core.Tests
{
    [TestFixture]
    public class CommandDispatcherTests
    {
        private CommandDispatcher _commandDispatcher;

        [SetUp]
        public void SetUp()
        {
            _commandDispatcher = CreateCommandDispatcher();
        }

        [Test]
        public void ExceptionThrownIfMoreThanOneHandlerCanHandleCommand()
        {
            Assert.That(() => _commandDispatcher.Dispatch(
                new TestCommand
                {
                    Id = Guid.NewGuid()
                }),
                Throws.Exception.InstanceOf<ArgumentException>().With.Message.EqualTo($"More than one type found that can handle {typeof(TestCommand).FullName} commands"));
        }

        [Test]
        public void ExceptionThrownIfNothingCanHandleCommand()
        {
            Assert.That(() => _commandDispatcher.Dispatch(
                new UnhandledTestCommand
                {
                    Id = Guid.NewGuid()
                }),
                Throws.Exception.InstanceOf<ArgumentException>().With.Message.EqualTo($"Could not find any handler to handle command of type {typeof(UnhandledTestCommand)}"));
        }

        [Test]
        public void ExceptionThrownIfCommandDoesNotHaveIdSet()
        {
            Assert.That(() => _commandDispatcher.Dispatch(
                new TestCommand()),
                Throws.Exception.InstanceOf<ArgumentException>().With.Message.EqualTo("Command does not have Id set."));
        }

        private CommandDispatcher CreateCommandDispatcher()
        {
            return new CommandDispatcher(null,
                new CommandHandlerProvider(new ICommandHandler[] { new Handler1(), new Handler2() }));
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

        internal class UnhandledTestCommand : ICommand
        {
            public Guid Id { get; set; }
            public Guid AggregateId { get; set; }
        }
    }
}