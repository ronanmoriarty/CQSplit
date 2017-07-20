using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CQRSTutorial.Core.Tests
{
    [TestFixture]
    public class CommandDispatcherTests
    {
        [Test]
        public void ExceptionThrownIfMoreThanOneHandlerTriesToHandleAnyGivenCommand()
        {
            Assert.That(() => new CommandDispatcher(null, new object[] { new Handler1(), new Handler1() }, new TypeInspector()), Throws.Exception.InstanceOf<ArgumentException>().With.Message.EqualTo($"More than one handler found for {typeof(TestCommand).FullName}"));
        }

        internal class Handler1 : ICommandHandler<TestCommand>
        {
            public IEnumerable<IEvent> Handle(TestCommand command)
            {
                throw new NotImplementedException();
            }
        }

        internal class TestCommand
        {
        }
    }
}