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

            Assert.That(() => _commandDispatcher.Dispatch(new TestCommand()),
                Throws.Exception.InstanceOf<ArgumentException>().With.Message.EqualTo($"More than one type found that can handle {typeof(TestCommand).FullName} commands"));
        }

        private CommandDispatcher CreateCommandDispatcher()
        {
            return new CommandDispatcher(null,
                new ICommandHandler[] { new Handler1(), new Handler2() },
                new TypeInspector(),
                new[] { typeof(Handler1), typeof(Handler2) });
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
        }
    }
}