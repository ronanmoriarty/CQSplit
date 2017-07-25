using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace CQRSTutorial.Core.Tests
{
    [TestFixture]
    public class CommandDispatcherTests
    {
        [Test]
        public void ExceptionThrownIfMoreThanOneTypeCanHandleAnyGivenCommand()
        {
            Assert.That(() => new CommandDispatcher(null, new ICommandHandler[] { new Handler1(), new Handler2() }, new TypeInspector(), Assembly.GetExecutingAssembly()), Throws.Exception.InstanceOf<ArgumentException>().With.Message.EqualTo($"More than one type found that can handle {typeof(TestCommand).FullName} commands"));
        }

        internal class Handler1 : ICommandHandler<TestCommand>
        {
            public IEnumerable<IEvent> Handle(TestCommand command)
            {
                throw new NotImplementedException();
            }

            public int Id { get; set; }
        }

        internal class Handler2 : ICommandHandler<TestCommand>
        {
            public IEnumerable<IEvent> Handle(TestCommand command)
            {
                throw new NotImplementedException();
            }

            public int Id { get; set; }
        }

        internal class TestCommand
        {
        }
    }
}