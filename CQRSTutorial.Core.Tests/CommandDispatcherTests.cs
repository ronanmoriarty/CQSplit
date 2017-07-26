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
        public void ExceptionThrownIfMoreThanOneTypeCanHandleAnyGivenCommand()
        {

            Assert.That(() => _commandDispatcher.Dispatch(
                new TestCommand
                {
                    Id = Guid.NewGuid()
                }),
                Throws.Exception.InstanceOf<ArgumentException>().With.Message.EqualTo($"More than one type found that can handle {typeof(TestCommand).FullName} commands"));
        }

        [Test]
        public void ExceptionThrownIfCommandDoesNotHaveIdSet()
        {

            Assert.That(() => _commandDispatcher.Dispatch(
                new TestCommand()),
                Throws.Exception.InstanceOf<ArgumentException>().With.Message.EqualTo("At least one command does not have Id set."));
        }

        private CommandDispatcher CreateCommandDispatcher()
        {
            return new CommandDispatcher(null,
                new AggregateStore(new ICommandHandler[] { new Handler1(), new Handler2() }),
                new TypeInspector());
        }

        internal class Handler1 : ICommandHandler<TestCommand>
        {
            public IEnumerable<IEvent> Handle(TestCommand command)
            {
                throw new NotImplementedException();
            }

            public bool CanHandle(ICommand command)
            {
                return true;
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
                return true;
            }
        }

        internal class TestCommand : ICommand
        {
            public Guid Id { get; set; }
            public Guid AggregateId { get; set; }
        }
    }
}